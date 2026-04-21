//-----------------------------------------------------------------------
// <copyright file="CheckListGrid.cs" company="Luppes Consulting, Inc.">
// Copyright 2019, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// CheckList Grid
// </summary>
//-----------------------------------------------------------------------

using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace CheckListApp.Data
{
  /// <summary>
  /// CheckList Grid Records
  /// </summary>
  public class CheckListGrid
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
    public List<CheckList> Data { get; set; }

    /// <summary>
    /// Return Message
    /// </summary>
    public string ReturnMessageTxt { get; set; }

    /// <summary>
    /// Initialize Model
    /// </summary>
    public CheckListGrid()
    {
      Total = 0;
    }

    /// <summary>
    /// Initialize Model
    /// </summary>
    /// <param name="data">Data Records</param>
    /// <param name="count">Total Record Count</param>
    public CheckListGrid(List<CheckList> data, int count)
    {
      Data = data;
      Total = count;
    }
  }
}    
