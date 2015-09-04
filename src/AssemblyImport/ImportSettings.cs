using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using NReflect.Filter;

namespace NClass.AssemblyImport
{
  /// <summary>
  /// This is needed to serialize an ArrayList of ImportSettings.
  /// </summary>
  [XmlInclude(typeof(ImportSettings))]
  public class TemplateList : ArrayList
  {
  }

  /// <summary>
  /// The settings in this class describe what and how to import.
  /// </summary>
  [Serializable]
  public class ImportSettings
  {
    #region === Properties

    /// <summary>
    /// Gets or sets the name of this Settings
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets a value indicating if the rules should be used as a white list.
    /// </summary>
    public bool UseAsWhiteList { get; set; }

    /// <summary>
    /// Gets or sets the list of <see cref="FilterRule"/> of the settings.
    /// </summary>
    public List<FilterRule> FilterRules { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the to create relationships.
    /// </summary>
    public bool CreateRelationships { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the to create association relationships.
    /// </summary>
    public bool CreateAssociations { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the to create realization relationships.
    /// </summary>
    public bool CreateRealizations { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the to create generalization relationships.
    /// </summary>
    public bool CreateGeneralizations { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the to create nesting relationships.
    /// </summary>
    public bool CreateNestings { get; set; }

    /// <summary>
    /// Gets a value indicating whether the fields used for an aggregation should be removed.
    /// </summary>
    public bool RemoveFields { get; set; }

    /// <summary>
    /// Gets a value indicating whether the aggregations should be labeled with the fieldname.
    /// </summary>
    public bool LabelAggregations { get; set; }

    #endregion

    #region === Methods

    /// <summary>
    /// Returns the name of the ImportSettings.
    /// </summary>
    /// <returns>The name of the ImportSettings.</returns>
    public override string ToString()
    {
      return Name;
    }

    #endregion
  }
}
