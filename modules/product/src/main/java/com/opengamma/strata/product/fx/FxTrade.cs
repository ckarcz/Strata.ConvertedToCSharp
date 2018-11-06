/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.fx
{

	/// <summary>
	/// A foreign exchange trade, such as an FX forward, FX spot or FX option.
	/// <para>
	/// FX trades operate on two different currencies.
	/// For example, it might represent the payment of USD 1,000 and the receipt of EUR 932.
	/// </para>
	/// </summary>
	public interface FxTrade : ProductTrade
	{

	  FxTrade withInfo(TradeInfo info);

	  FxProduct Product {get;}

	}

}