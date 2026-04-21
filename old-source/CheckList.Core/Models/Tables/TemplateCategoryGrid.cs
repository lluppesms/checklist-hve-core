//-----------------------------------------------------------------------
// <copyright file="TemplateCategoryGrid.cs" company="Luppes Consulting, Inc.">
// Copyright 2019, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// TemplateCategory Grid
// </summary>
//-----------------------------------------------------------------------

using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace CheckListApp.Data
{
  /// <summary>
  /// TemplateCategory Grid Records
  /// </summary>
  public class TemplateCategoryGrid
  {
    /// <summary>
    /// Total Record Count
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public int Total { get; set; }

    /// <summary>
    /// Data Records
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public List<TemplateCategory> Data { get; set; }

    /// <summary>
    /// Return Message
    /// </summary>
    public string ReturnMessageTxt { get; set; }

    /// <summary>
    /// Initialize Model
    /// </summary>
    public TemplateCategoryGrid()
    {
      Total = 0;
    }

    /// <summary>
    /// Initialize Model
    /// </summary>
    /// <param name="data">Data Records</param>
    /// <param name="count">Total Record Count</param>
    public TemplateCategoryGrid(List<TemplateCategory> data, int count)
    {
      Data = data;
      Total = count;
    }
  }
}    
