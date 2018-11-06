using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
/*
 * This code is copied from the original library from the `cern.jet.random.engine` package.
 * Changes:
 * - package name
 * - added serialization version
 * - reformat
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
	/// Same as <tt>MersenneTwister</tt> except that method <tt>raw()</tt> returns 64 bit random numbers instead of 32 bit random numbers.
	/// 
	/// @author wolfgang.hoschek@cern.ch
	/// @version 1.0, 09/24/99 </summary>
	/// <seealso cref= MersenneTwister </seealso>
	[Serializable]
	public class MersenneTwister64 : MersenneTwister
	{
	  private new const long serialVersionUID = 1L;

	  /// <summary>
	  /// Constructs and returns a random number generator with a default seed, which is a <b>constant</b>.
	  /// </summary>
	  public MersenneTwister64() : base()
	  {
	  }

	  /// <summary>
	  /// Constructs and returns a random number generator with the given seed. </summary>
	  /// <param name="seed"> should not be 0, in such a case <tt>MersenneTwister64.DEFAULT_SEED</tt> is silently substituted. </param>
	  public MersenneTwister64(int seed) : base(seed)
	  {
	  }

	  /// <summary>
	  /// Constructs and returns a random number generator seeded with the given date.
	  /// </summary>
	  /// <param name="d"> typically <tt>new java.util.Date()</tt> </param>
	  public MersenneTwister64(DateTime d) : base(d)
	  {
	  }

	  /// <summary>
	  /// Returns a 64 bit uniformly distributed random number in the open unit interval <code>(0.0,1.0)</code> (excluding 0.0 and 1.0).
	  /// </summary>
	  public override double raw()
	  {
		return nextDouble();
	  }
	}

}