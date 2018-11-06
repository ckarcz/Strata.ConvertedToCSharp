﻿using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
/*
 * This code is copied from the original library from the `cern.colt` package.
 * Changes:
 * - package name
 * - added serialization version
 * - reformat
 * - make package scoped
 */
/*
Copyright � 1999 CERN - European Organization for Nuclear Research.
Permission to use, copy, modify, distribute and sell this software and its documentation for any purpose
is hereby granted without fee, provided that the above copyright notice appear in all copies and
that both that copyright notice and this permission notice appear in supporting documentation.
CERN makes no representations about the suitability of this software for any purpose.
It is provided "as is" without expressed or implied warranty.
*/
namespace com.opengamma.strata.math.impl.cern
{
	//CSOFF: ALL
	/// <summary>
	/// This empty class is the common root for all persistent capable classes.
	/// If this class inherits from <tt>java.lang.Object</tt> then all subclasses are serializable with the standard Java serialization mechanism.
	/// If this class inherits from <tt>com.objy.db.app.ooObj</tt> then all subclasses are <i>additionally</i> serializable with the Objectivity ODBMS persistance mechanism.
	/// Thus, by modifying the inheritance of this class the entire tree of subclasses can be switched to Objectivity compatibility (and back) with minimum effort.
	/// </summary>
	[Serializable]
	internal abstract class PersistentObject : object, ICloneable
	{
	  public const long serialVersionUID = 1020L;

	  /// <summary>
	  /// Not yet commented.
	  /// </summary>
	  protected internal PersistentObject()
	  {
	  }

	  /// <summary>
	  /// Returns a copy of the receiver.
	  /// This default implementation does not nothing except making the otherwise <tt>protected</tt> clone method <tt>public</tt>.
	  /// </summary>
	  /// <returns> a copy of the receiver. </returns>
	  public override object clone()
	  {
		try
		{
		  return base.clone();
		}
		catch (CloneNotSupportedException)
		{
		  throw new InternalError(); //should never happen since we are cloneable
		}
	  }
	}

}