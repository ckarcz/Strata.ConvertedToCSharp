using System;

/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product
{
	using FromString = org.joda.convert.FromString;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using TypedString = com.opengamma.strata.collect.TypedString;
	using Bill = com.opengamma.strata.product.bond.Bill;
	using BondFuture = com.opengamma.strata.product.bond.BondFuture;
	using BondFutureOption = com.opengamma.strata.product.bond.BondFutureOption;
	using CapitalIndexedBond = com.opengamma.strata.product.bond.CapitalIndexedBond;
	using FixedCouponBond = com.opengamma.strata.product.bond.FixedCouponBond;
	using IborCapFloor = com.opengamma.strata.product.capfloor.IborCapFloor;
	using Cms = com.opengamma.strata.product.cms.Cms;
	using Cds = com.opengamma.strata.product.credit.Cds;
	using CdsIndex = com.opengamma.strata.product.credit.CdsIndex;
	using TermDeposit = com.opengamma.strata.product.deposit.TermDeposit;
	using Dsf = com.opengamma.strata.product.dsf.Dsf;
	using EtdFutureSecurity = com.opengamma.strata.product.etd.EtdFutureSecurity;
	using EtdOptionSecurity = com.opengamma.strata.product.etd.EtdOptionSecurity;
	using Fra = com.opengamma.strata.product.fra.Fra;
	using FxNdf = com.opengamma.strata.product.fx.FxNdf;
	using FxSingle = com.opengamma.strata.product.fx.FxSingle;
	using FxSwap = com.opengamma.strata.product.fx.FxSwap;
	using FxSingleBarrierOption = com.opengamma.strata.product.fxopt.FxSingleBarrierOption;
	using FxVanillaOption = com.opengamma.strata.product.fxopt.FxVanillaOption;
	using IborFuture = com.opengamma.strata.product.index.IborFuture;
	using IborFutureOption = com.opengamma.strata.product.index.IborFutureOption;
	using OvernightFuture = com.opengamma.strata.product.index.OvernightFuture;
	using BulletPayment = com.opengamma.strata.product.payment.BulletPayment;
	using Swap = com.opengamma.strata.product.swap.Swap;
	using Swaption = com.opengamma.strata.product.swaption.Swaption;

	/// <summary>
	/// The type of a portfolio item.
	/// <para>
	/// This provides a classification of the trade or position.
	/// </para>
	/// </summary>
	[Serializable]
	public sealed class ProductType : TypedString<ProductType>
	{

	  /// <summary>
	  /// A <seealso cref="BulletPayment"/>.
	  /// </summary>
	  public static readonly ProductType BULLET_PAYMENT = ProductType.of("BulletPayment", "Payment");
	  /// <summary>
	  /// A <seealso cref="Bill"/>.
	  /// </summary>
	  public static readonly ProductType BILL = ProductType.of("Bill");
	  /// <summary>
	  /// A <seealso cref="FixedCouponBond"/> or <seealso cref="CapitalIndexedBond"/>.
	  /// </summary>
	  public static readonly ProductType BOND = ProductType.of("Bond");
	  /// <summary>
	  /// A <seealso cref="BondFuture"/>.
	  /// </summary>
	  public static readonly ProductType BOND_FUTURE = ProductType.of("BondFuture", "Bond Future");
	  /// <summary>
	  /// A <seealso cref="BondFutureOption"/>.
	  /// </summary>
	  public static readonly ProductType BOND_FUTURE_OPTION = ProductType.of("BondFutureOption", "Bond Future Option");
	  /// <summary>
	  /// A <seealso cref="Cds"/>.
	  /// </summary>
	  public static readonly ProductType CDS = ProductType.of("Cds", "CDS");
	  /// <summary>
	  /// A <seealso cref="CdsIndex"/>.
	  /// </summary>
	  public static readonly ProductType CDS_INDEX = ProductType.of("Cds Index", "CDS Index");
	  /// <summary>
	  /// A <seealso cref="Cms"/>.
	  /// </summary>
	  public static readonly ProductType CMS = ProductType.of("Cms", "CMS");
	  /// <summary>
	  /// A <seealso cref="Dsf"/>.
	  /// </summary>
	  public static readonly ProductType DSF = ProductType.of("Dsf", "DSF");
	  /// <summary>
	  /// A <seealso cref="Fra"/>.
	  /// </summary>
	  public static readonly ProductType FRA = ProductType.of("Fra", "FRA");
	  /// <summary>
	  /// A <seealso cref="FxNdf"/>.
	  /// </summary>
	  public static readonly ProductType FX_NDF = ProductType.of("FxNdf", "FX NDF");
	  /// <summary>
	  /// A <seealso cref="FxSingleBarrierOption"/>.
	  /// </summary>
	  public static readonly ProductType FX_SINGLE_BARRIER_OPTION = ProductType.of("FxSingleBarrierOption", "FX Single Barrier Option");
	  /// <summary>
	  /// A <seealso cref="FxSingle"/>.
	  /// </summary>
	  public static readonly ProductType FX_SINGLE = ProductType.of("FxSingle", "FX");
	  /// <summary>
	  /// A <seealso cref="FxSwap"/>.
	  /// </summary>
	  public static readonly ProductType FX_SWAP = ProductType.of("FxSwap", "FX Swap");
	  /// <summary>
	  /// A <seealso cref="FxVanillaOption"/>.
	  /// </summary>
	  public static readonly ProductType FX_VANILLA_OPTION = ProductType.of("FxVanillaOption", "FX Vanilla Option");
	  /// <summary>
	  /// A <seealso cref="IborCapFloor"/>.
	  /// </summary>
	  public static readonly ProductType IBOR_CAP_FLOOR = ProductType.of("IborCapFloor", "Cap/Floor");
	  /// <summary>
	  /// A <seealso cref="IborFuture"/>.
	  /// </summary>
	  public static readonly ProductType IBOR_FUTURE = ProductType.of("IborFuture", "STIR Future");
	  /// <summary>
	  /// A <seealso cref="IborFutureOption"/>.
	  /// </summary>
	  public static readonly ProductType IBOR_FUTURE_OPTION = ProductType.of("IborFutureOption", "STIR Future Option");
	  /// <summary>
	  /// A <seealso cref="OvernightFuture"/>.
	  /// </summary>
	  public static readonly ProductType OVERNIGHT_FUTURE = ProductType.of("OvernightFuture", "Overnight Future");
	  /// <summary>
	  /// A representation based on sensitivities.
	  /// </summary>
	  public static readonly ProductType SENSITIVITIES = ProductType.of("Sensitivities");
	  /// <summary>
	  /// A <seealso cref="Swap"/>.
	  /// </summary>
	  public static readonly ProductType SWAP = ProductType.of("Swap");
	  /// <summary>
	  /// A <seealso cref="Swaption"/>.
	  /// </summary>
	  public static readonly ProductType SWAPTION = ProductType.of("Swaption");
	  /// <summary>
	  /// A <seealso cref="TermDeposit"/>.
	  /// </summary>
	  public static readonly ProductType TERM_DEPOSIT = ProductType.of("TermDeposit", "Deposit");
	  /// <summary>
	  /// An <seealso cref="EtdFutureSecurity"/>.
	  /// </summary>
	  public static readonly ProductType ETD_FUTURE = ProductType.of("EtdFuture", "ETD Future");
	  /// <summary>
	  /// A <seealso cref="EtdOptionSecurity"/>.
	  /// </summary>
	  public static readonly ProductType ETD_OPTION = ProductType.of("EtdOption", "ETD Option");
	  /// <summary>
	  /// A <seealso cref="Security"/>, used where the kind of security is not known.
	  /// </summary>
	  public static readonly ProductType SECURITY = ProductType.of("Security");
	  /// <summary>
	  /// A product only used for calibration.
	  /// </summary>
	  public static readonly ProductType CALIBRATION = ProductType.of("Calibration");
	  /// <summary>
	  /// Another kind of product, details not known.
	  /// </summary>
	  public static readonly ProductType OTHER = ProductType.of("Other");

	  /// <summary>
	  /// Serialization version. </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// The description.
	  /// </summary>
	  private readonly string description;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the specified name.
	  /// <para>
	  /// The name may contain any character, but must not be empty.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the name </param>
	  /// <returns> a type instance with the specified name </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static ProductType of(String name)
	  public static ProductType of(string name)
	  {
		return new ProductType(name, name);
	  }

	  /// <summary>
	  /// Obtains an instance from the specified name.
	  /// <para>
	  /// The name may contain any character, but must not be empty.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the name </param>
	  /// <param name="description">  the description </param>
	  /// <returns> a type instance with the specified name </returns>
	  private static ProductType of(string name, string description)
	  {
		return new ProductType(name, description);
	  }

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="name">  the name </param>
	  /// <param name="description">  the description </param>
	  private ProductType(string name, string description) : base(name)
	  {
		this.description = ArgChecker.notBlank(description, "description");
	  }

	  /// <summary>
	  /// Gets the human-readable description of the type.
	  /// </summary>
	  /// <returns> the description </returns>
	  public string Description
	  {
		  get
		  {
			return description;
		  }
	  }

	}

}