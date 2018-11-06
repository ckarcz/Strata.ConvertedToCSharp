using System;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.observable
{

	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using TypedMetaBean = org.joda.beans.TypedMetaBean;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using LightMetaBean = org.joda.beans.impl.light.LightMetaBean;

	using StandardId = com.opengamma.strata.basics.StandardId;
	using FieldName = com.opengamma.strata.data.FieldName;
	using ObservableId = com.opengamma.strata.data.ObservableId;
	using ObservableSource = com.opengamma.strata.data.ObservableSource;

	/// <summary>
	/// An identifier used to access a market quote.
	/// <para>
	/// A quote ID identifies a piece of data in an external data provider.
	/// </para>
	/// <para>
	/// Where possible, applications should use higher level IDs, instead of this class.
	/// Higher level market data keys allow the system to associate the market data with metadata when
	/// applying scenario definitions. If quote IDs are used directly, the system has no way to
	/// perturb the market data using higher level rules that rely on metadata.
	/// </para>
	/// <para>
	/// The <seealso cref="StandardId"/> in a quote ID is typically the ID from an underlying data provider (e.g.
	/// Bloomberg or Reuters). However the field name is a generic name which is mapped to the field name
	/// in the underlying provider by the market data system.
	/// </para>
	/// <para>
	/// The reason for this difference is the different sources of the ID and field name data. The ID is typically
	/// taken from an object which is provided to any calculations, for example a security linked to the
	/// trade. The calculation rarely has to make up an ID for an object it doesn't have access to.
	/// </para>
	/// <para>
	/// In contrast, calculations will often have to reference field names that aren't part their arguments. For
	/// example, if a calculation requires the last closing price of a security, it could take the ID from
	/// the security, but it needs a way to specify the field name containing the last closing price.
	/// </para>
	/// <para>
	/// If the field name were specific to the market data provider, the calculation would have to be aware
	/// of the source of its market data. However, if it uses a generic field name from {@code FieldNames}
	/// the market data source can change without affecting the calculation.
	/// 
	/// </para>
	/// </summary>
	/// <seealso cref= FieldName </seealso>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(style = "light", cacheHashCode = true) public final class QuoteId implements com.opengamma.strata.data.ObservableId, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class QuoteId : ObservableId, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.StandardId standardId;
		private readonly StandardId standardId;
	  /// <summary>
	  /// The field name in the market data record that contains the market data item.
	  /// The most common field name is <seealso cref="FieldName#MARKET_VALUE market value"/>.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.data.FieldName fieldName;
	  private readonly FieldName fieldName;
	  /// <summary>
	  /// The source of observable market data.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.data.ObservableSource observableSource;
	  private readonly ObservableSource observableSource;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance used to obtain an observable value.
	  /// <para>
	  /// The field name containing the data is <seealso cref="FieldName#MARKET_VALUE"/> and the market
	  /// data source is <seealso cref="ObservableSource#NONE"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="standardId">  the standard identifier of the data in the underlying data provider </param>
	  /// <returns> the identifier </returns>
	  public static QuoteId of(StandardId standardId)
	  {
		return new QuoteId(standardId, FieldName.MARKET_VALUE, ObservableSource.NONE);
	  }

	  /// <summary>
	  /// Obtains an instance used to obtain an observable value.
	  /// <para>
	  /// The market data source is <seealso cref="ObservableSource#NONE"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="standardId">  the standard identifier of the data in the underlying data provider </param>
	  /// <param name="fieldName">  the name of the field in the market data record holding the data </param>
	  /// <returns> the identifier </returns>
	  public static QuoteId of(StandardId standardId, FieldName fieldName)
	  {
		return new QuoteId(standardId, fieldName, ObservableSource.NONE);
	  }

	  /// <summary>
	  /// Obtains an instance used to obtain an observable value,
	  /// specifying the source of observable market data.
	  /// </summary>
	  /// <param name="standardId">  the standard identifier of the data in the underlying data provider </param>
	  /// <param name="fieldName">  the name of the field in the market data record holding the data </param>
	  /// <param name="obsSource">  the source of observable market data </param>
	  /// <returns> the identifier </returns>
	  public static QuoteId of(StandardId standardId, FieldName fieldName, ObservableSource obsSource)
	  {
		return new QuoteId(standardId, fieldName, obsSource);
	  }

	  //-------------------------------------------------------------------------
	  public QuoteId withObservableSource(ObservableSource obsSource)
	  {
		return new QuoteId(standardId, fieldName, obsSource);
	  }

	  public override string ToString()
	  {
		return (new StringBuilder(32)).Append("QuoteId:").Append(standardId).Append('/').Append(fieldName).Append(observableSource.Equals(ObservableSource.NONE) ? "" : "/" + observableSource).ToString();
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code QuoteId}.
	  /// </summary>
	  private static readonly TypedMetaBean<QuoteId> META_BEAN = LightMetaBean.of(typeof(QuoteId), MethodHandles.lookup(), new string[] {"standardId", "fieldName", "observableSource"}, new object[0]);

	  /// <summary>
	  /// The meta-bean for {@code QuoteId}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static TypedMetaBean<QuoteId> meta()
	  {
		return META_BEAN;
	  }

	  static QuoteId()
	  {
		MetaBean.register(META_BEAN);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// The cached hash code, using the racy single-check idiom.
	  /// </summary>
	  [NonSerialized]
	  private int cacheHashCode;

	  private QuoteId(StandardId standardId, FieldName fieldName, ObservableSource observableSource)
	  {
		JodaBeanUtils.notNull(standardId, "standardId");
		JodaBeanUtils.notNull(fieldName, "fieldName");
		JodaBeanUtils.notNull(observableSource, "observableSource");
		this.standardId = standardId;
		this.fieldName = fieldName;
		this.observableSource = observableSource;
	  }

	  public override TypedMetaBean<QuoteId> metaBean()
	  {
		return META_BEAN;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the identifier of the data.
	  /// This is typically an identifier from an external data provider. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public StandardId StandardId
	  {
		  get
		  {
			return standardId;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the field name in the market data record that contains the market data item.
	  /// The most common field name is <seealso cref="FieldName#MARKET_VALUE market value"/>. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public FieldName FieldName
	  {
		  get
		  {
			return fieldName;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the source of observable market data. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ObservableSource ObservableSource
	  {
		  get
		  {
			return observableSource;
		  }
	  }

	  //-----------------------------------------------------------------------
	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj != null && obj.GetType() == this.GetType())
		{
		  QuoteId other = (QuoteId) obj;
		  return JodaBeanUtils.equal(standardId, other.standardId) && JodaBeanUtils.equal(fieldName, other.fieldName) && JodaBeanUtils.equal(observableSource, other.observableSource);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = cacheHashCode;
		if (hash == 0)
		{
		  hash = this.GetType().GetHashCode();
		  hash = hash * 31 + JodaBeanUtils.GetHashCode(standardId);
		  hash = hash * 31 + JodaBeanUtils.GetHashCode(fieldName);
		  hash = hash * 31 + JodaBeanUtils.GetHashCode(observableSource);
		  cacheHashCode = hash;
		}
		return hash;
	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}