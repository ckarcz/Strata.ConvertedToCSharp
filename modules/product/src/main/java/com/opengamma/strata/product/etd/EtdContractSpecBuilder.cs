using System.Collections.Generic;

/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.etd
{

	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;

	using ExchangeId = com.opengamma.strata.product.common.ExchangeId;

	/// <summary>
	/// A builder for building instances of <seealso cref="EtdContractSpec"/>.
	/// </summary>
	public sealed class EtdContractSpecBuilder
	{

	  /// <summary>
	  /// The ID of the template. </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private EtdContractSpecId id_Renamed;

	  /// <summary>
	  /// The code of the product as given by the exchange in clearing and margining. </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private EtdContractCode contractCode_Renamed;

	  /// <summary>
	  /// The type of the product. </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private EtdType type_Renamed;

	  /// <summary>
	  /// The ID of the exchange where the instruments derived from the product are traded. </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private ExchangeId exchangeId_Renamed;

	  /// <summary>
	  /// The description of the product. </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private string description_Renamed;

	  /// <summary>
	  /// The information about the security price - currency, tick size, tick value, contract size. </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private SecurityPriceInfo priceInfo_Renamed;

	  /// <summary>
	  /// The attributes.
	  /// <para>
	  /// Security attributes, provide the ability to associate arbitrary information
	  /// with a security template in a key-value map.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<com.opengamma.strata.product.AttributeType<?>, Object> attributes = new java.util.HashMap<>();
	  private readonly IDictionary<AttributeType<object>, object> attributes = new Dictionary<AttributeType<object>, object>();

	  /// <summary>
	  /// Creates an empty builder.
	  /// </summary>
	  internal EtdContractSpecBuilder()
	  {
	  }

	  /// <summary>
	  /// Builds a new specification from the data in this builder.
	  /// </summary>
	  /// <returns> a specification instance built from the data in this builder </returns>
	  public EtdContractSpec build()
	  {
		if (id_Renamed == null)
		{
		  id_Renamed = EtdIdUtils.contractSpecId(type_Renamed, exchangeId_Renamed, contractCode_Renamed);
		}
		return new EtdContractSpec(id_Renamed, type_Renamed, exchangeId_Renamed, contractCode_Renamed, description_Renamed, priceInfo_Renamed, attributes);
	  }

	  /// <summary>
	  /// Sets the ID of the contract specification.
	  /// </summary>
	  /// <param name="id"> the ID </param>
	  /// <returns> the ID of the template </returns>
	  public EtdContractSpecBuilder id(EtdContractSpecId id)
	  {
		JodaBeanUtils.notNull(id, "id");
		this.id_Renamed = id;
		return this;
	  }

	  /// <summary>
	  /// Sets the type of the contract specification.
	  /// </summary>
	  /// <param name="productType">  the new value, not null </param>
	  /// <returns> this, for chaining, not null </returns>
	  public EtdContractSpecBuilder type(EtdType productType)
	  {
		JodaBeanUtils.notNull(productType, "productType");
		this.type_Renamed = productType;
		return this;
	  }

	  /// <summary>
	  /// Sets the ID of the exchange where the instruments derived from the contract specification are traded.
	  /// </summary>
	  /// <param name="exchangeId">  the new value, not null </param>
	  /// <returns> this, for chaining, not null </returns>
	  public EtdContractSpecBuilder exchangeId(ExchangeId exchangeId)
	  {
		JodaBeanUtils.notNull(exchangeId, "exchangeId");
		this.exchangeId_Renamed = exchangeId;
		return this;
	  }

	  /// <summary>
	  /// Sets the code of the contract specification as given by the exchange in clearing and margining.
	  /// </summary>
	  /// <param name="contractCode">  the new value, not empty </param>
	  /// <returns> this, for chaining, not null </returns>
	  public EtdContractSpecBuilder contractCode(EtdContractCode contractCode)
	  {
		JodaBeanUtils.notNull(contractCode, "contractCode");
		this.contractCode_Renamed = contractCode;
		return this;
	  }

	  /// <summary>
	  /// Sets the description of the contract specification.
	  /// </summary>
	  /// <param name="description">  the new value, not empty </param>
	  /// <returns> this, for chaining, not null </returns>
	  public EtdContractSpecBuilder description(string description)
	  {
		JodaBeanUtils.notEmpty(description, "description");
		this.description_Renamed = description;
		return this;
	  }

	  /// <summary>
	  /// Sets the information about the security price - currency, tick size, tick value, contract size.
	  /// </summary>
	  /// <param name="priceInfo">  the new value, not null </param>
	  /// <returns> this, for chaining, not null </returns>
	  public EtdContractSpecBuilder priceInfo(SecurityPriceInfo priceInfo)
	  {
		JodaBeanUtils.notNull(priceInfo, "priceInfo");
		this.priceInfo_Renamed = priceInfo;
		return this;
	  }

	  /// <summary>
	  /// Adds an attribute to the builder.
	  /// <para>
	  /// Only one attribute is stored for each attribute type. If this method is called multiple times with the
	  /// same attribute type the previous attribute value will be replaced.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="attributeType"> the type of the attribute </param>
	  /// <param name="attributeValue"> the value of the attribute </param>
	  /// @param <T> the type of the attribute </param>
	  /// <returns> this builder </returns>
	  public EtdContractSpecBuilder addAttribute<T>(AttributeType<T> attributeType, T attributeValue)
	  {
		JodaBeanUtils.notNull(attributeType, "attributeType");
		JodaBeanUtils.notNull(attributeValue, "attributeValue");
		attributes[attributeType] = attributeValue;
		return this;
	  }

	}

}