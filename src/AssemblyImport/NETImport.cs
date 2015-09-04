using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using NClass.AssemblyImport.Lang;
using NClass.Core;
using NClass.DiagramEditor.ClassDiagram;
using NClass.DiagramEditor.ClassDiagram.Connections;
using NClass.DiagramEditor.ClassDiagram.Shapes;
using NReflect;
using NReflect.Filter;
using NReflect.NREntities;
using NReflect.NRMembers;
using NReflect.NRParameters;
using NReflect.NRCode;
using NReflect.NRRelationship;

namespace NClass.AssemblyImport
{
  public class NETImport
  {
    // ========================================================================
    // Constants

    #region === Constants

    #endregion

    // ========================================================================
    // Fields

    #region === Fields

    /// <summary>
    /// The diagram to add the new entities to.
    /// </summary>
    private readonly Diagram diagram;

    /// <summary>
    /// An <see cref="ImportSettings"/> instance which describes which entities and members to reflect.
    /// </summary>
    private readonly ImportSettings settings;

    /// <summary>
    /// A mapping from NReflects <see cref="NRTypeBase"/> objects to the
    /// corresponding NClass <see cref="TypeBase"/> objects.
    /// </summary>
    private readonly Dictionary<NRTypeBase, TypeBase> types;

    #endregion

    // ========================================================================
    // Con- / Destruction

    #region === Con- / Destruction

    /// <summary>
    /// Initializes a new instance of <see cref="NETImport"/>.
    /// </summary>
    public NETImport(Diagram diagram, ImportSettings settings)
    {
      this.diagram = diagram;
      this.settings = settings;

      types = new Dictionary<NRTypeBase, TypeBase>();
    }

    #endregion

    // ========================================================================
    // Properties

    #region === Properties


    #endregion

    // ========================================================================
    // Methods

    #region === Methods

    /// <summary>
    /// The main entry point of this class. Imports the assembly which is given
    /// as the parameter.
    /// </summary>
    /// <param name="fileName">The file name and path of the assembly to import.</param>
    /// <returns><c>True</c>, if the import was successful.</returns>
    public bool ImportAssembly(string fileName)
    {
      if (string.IsNullOrEmpty(fileName))
      {
        MessageBox.Show(Strings.Error_NoAssembly, Strings.Error_MessageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }
      try
      {
        diagram.Name = Path.GetFileName(fileName);
        diagram.RedrawSuspended = true;

        IncludeFilter includeFilter = new IncludeFilter();
        includeFilter.Rules.AddRange(settings.FilterRules);
        IFilter filter = includeFilter;
        if(!settings.UseAsWhiteList)
        {
          filter = new InvertFilter(includeFilter);
        }

        NClassImportFilter nClassImportFilter = new NClassImportFilter(filter);
        Reflector reflector = new Reflector();
        filter = nClassImportFilter;
        NRAssembly nrAssembly = reflector.Reflect(fileName, ref filter);
        nClassImportFilter = (NClassImportFilter)filter;

        AddInterfaces(nrAssembly.Interfaces);
        AddClasses(nrAssembly.Classes);
        AddStrcts(nrAssembly.Structs);
        AddDelegates(nrAssembly.Delegates);
        AddEnums(nrAssembly.Enums);

        ArrangeTypes();

        RelationshipCreator relationshipCreator = new RelationshipCreator();
        NRRelationships nrRelationships = relationshipCreator.CreateRelationships(nrAssembly, settings.CreateNestings,
                                                                                  settings.CreateGeneralizations,
                                                                                  settings.CreateRealizations,
                                                                                  settings.CreateAssociations);
        AddRelationships(nrRelationships);

        if(nClassImportFilter.UnsafeTypesPresent)
        {
          MessageBox.Show(null, Strings.UnsafeTypesPresent, Strings.WarningTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
      }
      catch (ReflectionTypeLoadException)
      {
        MessageBox.Show(Strings.Error_MissingReferencedAssemblies, Strings.Error_MessageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }
      catch (FileLoadException)
      {
        MessageBox.Show(Strings.Error_MissingReferencedAssemblies, Strings.Error_MessageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }
      catch (BadImageFormatException)
      {
        MessageBox.Show(Strings.Error_BadImageFormat, Strings.Error_MessageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }
      catch (Exception ex)
      {
        MessageBox.Show(String.Format(Strings.Error_GeneralException, ex), Strings.Error_MessageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }
      finally
      {
        diagram.RedrawSuspended = false;
      }

      return true;
    }

    /// <summary>
    /// Adds the relationships from <paramref name="nrRelationships"/> to the
    /// diagram.
    /// </summary>
    /// <param name="nrRelationships">The relationships to add.</param>
    private void AddRelationships(NRRelationships nrRelationships)
    {
      foreach(NRNesting nrNesting in nrRelationships.Nestings)
      {
        CompositeType parentType = types[nrNesting.ParentType] as CompositeType;
        TypeBase innerType = types[nrNesting.InnerType];
        if(parentType != null && innerType != null)
        {
          diagram.AddNesting(parentType, innerType);
        }
      }

      foreach(NRGeneralization nrGeneralization in nrRelationships.Generalizations)
      {
        CompositeType derivedType = types[nrGeneralization.DerivedType] as CompositeType;
        CompositeType baseType = types[nrGeneralization.BaseType] as CompositeType;
        if(derivedType != null && baseType != null)
        {
          diagram.AddGeneralization(derivedType, baseType);
        }
      }

      foreach (NRRealization nrRealization in nrRelationships.Realizations)
      {
        CompositeType implementingType = types[nrRealization.ImplementingType] as CompositeType;
        InterfaceType interfaceType = types[nrRealization.BaseType] as InterfaceType;
        if (implementingType != null && interfaceType != null)
        {
          diagram.AddRealization(implementingType, interfaceType);
        }
      }

      foreach (NRAssociation nrAssociation in nrRelationships.Associations)
      {
        TypeBase first = types[nrAssociation.StartType];
        TypeBase second = types[nrAssociation.EndType];
        if (first != null && second != null)
        {
          AssociationRelationship associationRelationship = diagram.AddAssociation(first, second);
          associationRelationship.EndMultiplicity = nrAssociation.EndMultiplicity;
          associationRelationship.StartMultiplicity = nrAssociation.StartMultiplicity;
          associationRelationship.EndRole = nrAssociation.EndRole;
          associationRelationship.StartRole = nrAssociation.StartRole;
        }
      }
    }

    /// <summary>
    /// Creates a nice arrangement for each entity.
    /// </summary>
    private void ArrangeTypes()
    {
      const int Margin = Connection.Spacing * 2;
      const int DiagramPadding = Shape.SelectionMargin;

      int shapeCount = diagram.ShapeCount;
      int columns = (int)Math.Ceiling(Math.Sqrt(shapeCount * 2));
      int shapeIndex = 0;
      int top = Shape.SelectionMargin;
      int maxHeight = 0;

      foreach (Shape shape in diagram.Shapes)
      {
        int column = shapeIndex % columns;

        shape.Location = new Point(
          (TypeShape.DefaultWidth + Margin) * column + DiagramPadding, top);

        maxHeight = Math.Max(maxHeight, shape.Height);
        if (column == columns - 1)
        {
          top += maxHeight + Margin;
          maxHeight = 0;
        }
        shapeIndex++;
      }
    }

    #region --- Entities

    /// <summary>
    /// Adds the submitted classes to the diagram.
    /// </summary>
    /// <param name="classes">A list of classes to add.</param>
    private void AddClasses(IEnumerable<NRClass> classes)
    {
      foreach (NRClass nrClass in classes)
      {
        ClassType classType = diagram.AddClass();
        classType.Name = nrClass.Name;
        classType.AccessModifier = nrClass.AccessModifier.ToNClass();
        classType.Modifier = nrClass.ClassModifier.ToNClass();

        AddFields(classType, nrClass.Fields);
        AddProperties(classType, nrClass.Properties);
        AddEvents(classType, nrClass.Events);
        AddConstructors(classType, nrClass.Constructors);
        AddMethods(classType, nrClass.Methods);
        AddOperators(classType, nrClass.Operators);

        types.Add(nrClass, classType);
      }
    }

    /// <summary>
    /// Adds the submitted structs to the diagram.
    /// </summary>
    /// <param name="structs">A list of structs to add.</param>
    private void AddStrcts(IEnumerable<NRStruct> structs)
    {
      foreach (NRStruct nrStruct in structs)
      {
        StructureType structureType = diagram.AddStructure();
        structureType.Name = nrStruct.Name;
        structureType.AccessModifier = nrStruct.AccessModifier.ToNClass();

        AddFields(structureType, nrStruct.Fields);
        AddProperties(structureType, nrStruct.Properties);
        AddEvents(structureType, nrStruct.Events);
        AddConstructors(structureType, nrStruct.Constructors);
        AddMethods(structureType, nrStruct.Methods);
        AddOperators(structureType, nrStruct.Operators);

        types.Add(nrStruct, structureType);
      }
    }

    /// <summary>
    /// Adds the submitted interfaces to the diagram.
    /// </summary>
    /// <param name="interfaces">A list of interfaces to add.</param>
    private void AddInterfaces(IEnumerable<NRInterface> interfaces)
    {
      foreach (NRInterface nrInterface in interfaces)
      {
        InterfaceType interfaceType = diagram.AddInterface();
        interfaceType.Name = nrInterface.Name;
        interfaceType.AccessModifier = nrInterface.AccessModifier.ToNClass();

        AddProperties(interfaceType, nrInterface.Properties);
        AddEvents(interfaceType, nrInterface.Events);
        AddMethods(interfaceType, nrInterface.Methods);

        types.Add(nrInterface, interfaceType);
      }
    }

    /// <summary>
    /// Adds the submitted delegates to the diagram.
    /// </summary>
    /// <param name="delegates">A list of delegates to add.</param>
    private void AddDelegates(IEnumerable<NRDelegate> delegates)
    {
      foreach (NRDelegate nrDelegate in delegates)
      {
        DelegateType delegateType = diagram.AddDelegate();
        delegateType.Name = nrDelegate.Name;
        delegateType.AccessModifier = nrDelegate.AccessModifier.ToNClass();
        delegateType.ReturnType = nrDelegate.ReturnType;
        foreach(NRParameter nrParameter in nrDelegate.Parameters)
        {
          delegateType.AddParameter(nrParameter.Declaration());
        }

        types.Add(nrDelegate, delegateType);
      }
    }

    /// <summary>
    /// Adds the submitted enums to the diagram.
    /// </summary>
    /// <param name="enums">A list of enums to add.</param>
    private void AddEnums(IEnumerable<NREnum> enums)
    {
      foreach (NREnum nrEnum in enums)
      {
        EnumType enumType = diagram.AddEnum();
        enumType.Name = nrEnum.Name;
        enumType.AccessModifier = nrEnum.AccessModifier.ToNClass();

        AddEnumValues(enumType, nrEnum.Values);

        types.Add(nrEnum, enumType);
      }
    }

    #endregion

    #region --- Member

    /// <summary>
    /// Adds the given enum values to the given type.
    /// </summary>
    /// <param name="type">The enum to add the enum values to.</param>
    /// <param name="values">A list of enum values to add.</param>
    private void AddEnumValues(EnumType type, IEnumerable<NREnumValue> values)
    {
      foreach (NREnumValue nrEnumValue in values)
      {
        type.AddValue(nrEnumValue.Declaration());
      }
    }

    /// <summary>
    /// Adds the given fields to the given type.
    /// </summary>
    /// <param name="type">The entity to add the fields to.</param>
    /// <param name="fields">A list of fields to add.</param>
    private void AddFields(SingleInharitanceType type, IEnumerable<NRField> fields)
    {
      foreach (NRField nrField in fields)
      {
        type.AddField().InitFromString(nrField.Declaration());
      }
    }

    /// <summary>
    /// Adds the given properties to the given type.
    /// </summary>
    /// <param name="type">The entity to add the properties to.</param>
    /// <param name="properties">A list of properties to add.</param>
    private void AddProperties(CompositeType type, IEnumerable<NRProperty> properties)
    {
      foreach (NRProperty nrProperty in properties)
      {
        type.AddProperty().InitFromString(nrProperty.Declaration());
      }
    }

    /// <summary>
    /// Adds the given methods to the given type.
    /// </summary>
    /// <param name="type">The entity to add the methods to.</param>
    /// <param name="methods">A list of methods to add.</param>
    private void AddMethods(CompositeType type, IEnumerable<NRMethod> methods)
    {
      foreach (NRMethod nrMethod in methods)
      {
        type.AddMethod().InitFromString(nrMethod.Declaration());
      }
    }

    /// <summary>
    /// Adds the given constructors to the given type.
    /// </summary>
    /// <param name="type">The entity to add the constructors to.</param>
    /// <param name="constructors">A list of constructors to add.</param>
    private void AddConstructors(SingleInharitanceType type, IEnumerable<NRConstructor> constructors)
    {
      foreach (NRConstructor nrConstructor in constructors)
      {
        type.AddConstructor().InitFromString(nrConstructor.Declaration());
      }
    }

    /// <summary>
    /// Adds the given operators to the given type.
    /// </summary>
    /// <param name="type">The entity to add the operators to.</param>
    /// <param name="operators">A list of operators to add.</param>
    private void AddOperators(SingleInharitanceType type, IEnumerable<NROperator> operators)
    {
      foreach (NROperator nrOperator in operators)
      {
        type.AddMethod().InitFromString(nrOperator.Declaration());
      }
    }

    /// <summary>
    /// Adds the given events to the given type.
    /// </summary>
    /// <param name="type">The entity to add the events to.</param>
    /// <param name="events">A list of events to add.</param>
    private void AddEvents(CompositeType type, IEnumerable<NREvent> events)
    {
      foreach (NREvent nrEvent in events)
      {
        type.AddEvent().InitFromString(nrEvent.Declaration());
      }
    }

    #endregion

    #endregion
  }
}
