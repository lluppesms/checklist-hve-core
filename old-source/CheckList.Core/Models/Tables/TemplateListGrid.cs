//-----------------------------------------------------------------------
// <copyright file="TemplateListGrid.cs" company="Luppes Consulting, Inc.">
// Copyright 2019, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// TemplateList Grid
// </summary>
//-----------------------------------------------------------------------

using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace CheckListApp.Data
{
  /// <summary>
  /// TemplateList Grid Records
  /// </summary>
  public class TemplateListGrid
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
    public List<TemplateList> Data { get; set; }

    /// <summary>
    /// Return Message
    /// </summary>
    public string ReturnMessageTxt { get; set; }

    /// <summary>
    /// Initialize Model
    /// </summary>
    public TemplateListGrid()
    {
      Total = 0;
    }

    /// <summary>
    /// Initialize Model
    /// </summary>
    /// <param name="data">Data Records</param>
    /// <param name="count">Total Record Count</param>
    public TemplateListGrid(List<TemplateList> data, int count)
    {
      Data = data;
      Total = count;
    }
  }
}    
