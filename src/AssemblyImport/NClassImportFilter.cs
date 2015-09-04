using System;
using System.Collections.Generic;
using System.Linq;
using NReflect.Filter;
using NReflect.NREntities;
using NReflect.NRMembers;
using NReflect.NRParameters;

namespace NClass.AssemblyImport
{
  /// <summary>
  /// This import filter will filter out all entities and members NClass doesn't
  /// understand.
  /// </summary>
  [Serializable]
  public class NClassImportFilter : IFilter
  {
    // ========================================================================
    // Fields

    #region === Fields

    /// <summary>
    /// The filter to delegate filter calls to.
    /// </summary>
    private readonly IFilter filter;

    #endregion

    // ========================================================================
    // Con- / Destruction

    #region === Con- / Destruction

    /// <summary>
    /// Initializes a new instance of <see cref="NClassImportFilter"/>.
    /// </summary>
    /// <param name="filter">The filter to delegate filter calls to.</param>
    public NClassImportFilter(IFilter filter)
    {
      this.filter = filter;
      UnsafeTypesPresent = false;
    }

    #endregion

    // ========================================================================
    // Properties

    #region === Properties

    private bool unsafeTypesPresent;

    /// <summary>
    /// Gets or sets a value indicating if unsafe types where filtered out.
    /// </summary>
    public bool UnsafeTypesPresent
    {
      get { return unsafeTypesPresent; }
      private set { unsafeTypesPresent = value; }
    }

    #endregion

    // ========================================================================
    // Operators and Type Conversions

    #region === Operators and Type Conversions


    #endregion

    // ========================================================================
    // Methods

    #region === Methods

    /// <summary>
    /// Determines if a class will be reflected.
    /// </summary>
    /// <param name="nrClass">The class to test.</param>
    /// <returns>
    /// <c>True</c> if the class should be reflected.
    /// </returns>
    public bool Reflect(NRClass nrClass)
    {
      return filter.Reflect(nrClass);
    }

    /// <summary>
    /// Determines if an interface will be reflected.
    /// </summary>
    /// <param name="nrInterface">The interface to test.</param>
    /// <returns>
    /// <c>True</c> if the interface should be reflected.
    /// </returns>
    public bool Reflect(NRInterface nrInterface)
    {
      return filter.Reflect(nrInterface);
    }

    /// <summary>
    /// Determines if a struct will be reflected.
    /// </summary>
    /// <param name="nrStruct">The struct to test.</param>
    /// <returns>
    /// <c>True</c> if the struct should be reflected.
    /// </returns>
    public bool Reflect(NRStruct nrStruct)
    {
      return filter.Reflect(nrStruct);
    }

    /// <summary>
    /// Determines if a delegate will be reflected.
    /// </summary>
    /// <param name="nrDelegate">The delegate to test.</param>
    /// <returns>
    /// <c>True</c> if the delegate should be reflected.
    /// </returns>
    public bool Reflect(NRDelegate nrDelegate)
    {
      return IsUnsafePointer(nrDelegate.ReturnType) || HasUnsafeParameters(nrDelegate.Parameters) ? false : filter.Reflect(nrDelegate);
    }

    /// <summary>
    /// Determines if a enum will be reflected.
    /// </summary>
    /// <param name="nrEnum">The enum to test.</param>
    /// <returns>
    /// <c>True</c> if the enum should be reflected.
    /// </returns>
    public bool Reflect(NREnum nrEnum)
    {
      return filter.Reflect(nrEnum);
    }

    /// <summary>
    /// Determines if a enum value will be reflected.
    /// </summary>
    /// <param name="nrEnumValue">The enum value to test.</param>
    /// <returns>
    /// <c>True</c> if the enum value should be reflected.
    /// </returns>
    public bool Reflect(NREnumValue nrEnumValue)
    {
      return filter.Reflect(nrEnumValue);
    }

    /// <summary>
    /// Determines if a method will be reflected.
    /// </summary>
    /// <param name="nrConstructor">The method to test.</param>
    /// <returns>
    /// <c>True</c> if the method should be reflected.
    /// </returns>
    public bool Reflect(NRConstructor nrConstructor)
    {
      return HasUnsafeParameters(nrConstructor.Parameters) ? false : filter.Reflect(nrConstructor);
    }

    /// <summary>
    /// Determines if a method will be reflected.
    /// </summary>
    /// <param name="nrMethod">The method to test.</param>
    /// <returns>
    /// <c>True</c> if the method should be reflected.
    /// </returns>
    public bool Reflect(NRMethod nrMethod)
    {
      return IsUnsafePointer(nrMethod.Type) || HasUnsafeParameters(nrMethod.Parameters) ? false : filter.Reflect(nrMethod);
    }

    /// <summary>
    /// Determines if an operator will be reflected.
    /// </summary>
    /// <param name="nrOperator">The operator to test.</param>
    /// <returns>
    /// <c>True</c> if the operator should be reflected.
    /// </returns>
    public bool Reflect(NROperator nrOperator)
    {
      return IsUnsafePointer(nrOperator.Type) || HasUnsafeParameters(nrOperator.Parameters) ? false : filter.Reflect(nrOperator);
    }

    /// <summary>
    /// Determines if an event will be reflected.
    /// </summary>
    /// <param name="nrEvent">The event to test.</param>
    /// <returns>
    /// <c>True</c> if the event should be reflected.
    /// </returns>
    public bool Reflect(NREvent nrEvent)
    {
      return filter.Reflect(nrEvent);
    }

    /// <summary>
    /// Determines if a field will be reflected.
    /// </summary>
    /// <param name="nrField">The field to test.</param>
    /// <returns>
    /// <c>True</c> if the field should be reflected.
    /// </returns>
    public bool Reflect(NRField nrField)
    {
      return IsUnsafePointer(nrField.Type) ? false : filter.Reflect(nrField);
    }

    /// <summary>
    /// Determines if a property will be reflected.
    /// </summary>
    /// <param name="nrProperty">The property to test.</param>
    /// <returns>
    /// <c>True</c> if the property should be reflected.
    /// </returns>
    public bool Reflect(NRProperty nrProperty)
    {
      return IsUnsafePointer(nrProperty.Type) ? false : filter.Reflect(nrProperty);
    }

    /// <summary>
    /// Returns <c>true</c> if the given type represents an unsafe pointer.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns><c>true</c> if the given type represents an unsafe pointer.</returns>
    private bool IsUnsafePointer(string type)
    {
      if (type.Contains("*"))
      {
        UnsafeTypesPresent = true;
        return true;
      }
      return false;
    }

    /// <summary>
    /// Returns <c>true</c> if the given list of parameters contains an unsafe pointer.
    /// </summary>
    /// <param name="parameters">The list of parameters to check.</param>
    /// <returns><c>true</c> if the given list of parameters contains an unsafe pointer.</returns>
    private bool HasUnsafeParameters(IEnumerable<NRParameter> parameters)
    {
      return parameters.Any(parameter => IsUnsafePointer(parameter.Type));
    }

    #endregion

    // ========================================================================
    // Events

    #region === Events


    #endregion
  }
}