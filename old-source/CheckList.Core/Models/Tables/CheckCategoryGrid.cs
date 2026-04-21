//-----------------------------------------------------------------------
// <copyright file="CheckCategoryGrid.cs" company="Luppes Consulting, Inc.">
// Copyright 2019, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// CheckCategory Grid
// </summary>
//-----------------------------------------------------------------------

using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace CheckListApp.Data
{
  /// <summary>
  /// CheckCategory Grid Records
  /// </summary>
  public class CheckCategoryGrid
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
    public List<CheckCategory> Data { get; set; }

    /// <summary>
    /// Return Message
    /// </summary>
    public string ReturnMessageTxt { get; set; }

    /// <summary>
    /// Initialize Model
    /// </summary>
    public CheckCategoryGrid()
    {
      Total = 0;
    }

    /// <summary>
    /// Initialize Model
    /// </summary>
    /// <param name="data">Data Records</param>
    /// <param name="count">Total Record Count</param>
    public CheckCategoryGrid(List<CheckCategory> data, int count)
    {
      Data = data;
      Total = count;
    }
  }
}    
