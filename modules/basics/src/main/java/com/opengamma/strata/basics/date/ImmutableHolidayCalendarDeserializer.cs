using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.date
{

	using BeanBuilder = org.joda.beans.BeanBuilder;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BufferingBeanBuilder = org.joda.beans.impl.BufferingBeanBuilder;
	using StandaloneMetaProperty = org.joda.beans.impl.StandaloneMetaProperty;
	using DefaultDeserializer = org.joda.beans.ser.DefaultDeserializer;

	using TypeToken = com.google.common.reflect.TypeToken;
	using Meta = com.opengamma.strata.basics.date.ImmutableHolidayCalendar.Meta;

	/// <summary>
	/// Deserialize {@code ImmutableHolidayCalendar} handling old format.
	/// </summary>
	internal sealed class ImmutableHolidayCalendarDeserializer : DefaultDeserializer
	{

	  private static readonly Meta META_BEAN = ImmutableHolidayCalendar.meta();
	  private static readonly MetaProperty<HolidayCalendarId> ID = META_BEAN.id();
	  private static readonly MetaProperty<int> WEEKENDS = META_BEAN.weekends();
	  private static readonly MetaProperty<int> START_YEAR = META_BEAN.startYear();
	  private static readonly MetaProperty<int[]> LOOKUP = META_BEAN.lookup();

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"rawtypes", "unchecked", "serial"}) private static final org.joda.beans.MetaProperty<java.util.Set<java.time.LocalDate>> HOLIDAYS = (org.joda.beans.MetaProperty) org.joda.beans.impl.StandaloneMetaProperty.of("holidays", META_BEAN, java.util.Set.class, new com.google.common.reflect.TypeToken<java.util.Set<java.time.LocalDate>>()
	  private static readonly MetaProperty<ISet<LocalDate>> HOLIDAYS = (MetaProperty) StandaloneMetaProperty.of("holidays", META_BEAN, typeof(ISet<object>), new TypeTokenAnonymousInnerClass().Type);

	  private class TypeTokenAnonymousInnerClass : TypeToken<ISet<LocalDate>>
	  {
		  public TypeTokenAnonymousInnerClass()
		  {
		  }

	  }
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"rawtypes", "unchecked", "serial"}) private static final org.joda.beans.MetaProperty<java.util.Set<java.time.DayOfWeek>> WEEKEND_DAYS = (org.joda.beans.MetaProperty) org.joda.beans.impl.StandaloneMetaProperty.of("weekendDays", META_BEAN, java.util.Set.class, new com.google.common.reflect.TypeToken<java.util.Set<java.time.DayOfWeek>>()
	  private static readonly MetaProperty<ISet<DayOfWeek>> WEEKEND_DAYS = (MetaProperty) StandaloneMetaProperty.of("weekendDays", META_BEAN, typeof(ISet<object>), new TypeTokenAnonymousInnerClass2().Type);

	  private class TypeTokenAnonymousInnerClass2 : TypeToken<ISet<DayOfWeek>>
	  {
		  public TypeTokenAnonymousInnerClass2()
		  {
		  }

	  }

	  //-------------------------------------------------------------------------
	  // restricted constructor
	  internal ImmutableHolidayCalendarDeserializer()
	  {
	  }

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<?> createBuilder(Class beanType, org.joda.beans.MetaBean metaBean)
	  public override BeanBuilder<object> createBuilder(Type beanType, MetaBean metaBean)
	  {
		return BufferingBeanBuilder.of(metaBean);
	  }

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.MetaProperty<?> findMetaProperty(Class beanType, org.joda.beans.MetaBean metaBean, String propertyName)
	  public override MetaProperty<object> findMetaProperty(Type beanType, MetaBean metaBean, string propertyName)
	  {
		try
		{
		  return metaBean.metaProperty(propertyName);
		}
		catch (NoSuchElementException ex)
		{
		  if (HOLIDAYS.name().Equals(propertyName))
		  {
			return HOLIDAYS;
		  }
		  if (WEEKEND_DAYS.name().Equals(propertyName))
		  {
			return WEEKEND_DAYS;
		  }
		  throw ex;
		}
	  }

	  public override object build<T1>(Type beanType, BeanBuilder<T1> builder)
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.concurrent.ConcurrentMap<org.joda.beans.MetaProperty<?>, Object> buffer = ((org.joda.beans.impl.BufferingBeanBuilder<?>) builder).getBuffer();
		ConcurrentMap<MetaProperty<object>, object> buffer = ((BufferingBeanBuilder<object>) builder).Buffer;
		HolidayCalendarId id = builder.get(ID);
		if (buffer.containsKey(HOLIDAYS) && buffer.containsKey(WEEKEND_DAYS))
		{
		  ISet<LocalDate> holidays = builder.get(HOLIDAYS);
		  ISet<DayOfWeek> weekendDays = builder.get(WEEKEND_DAYS);
		  return ImmutableHolidayCalendar.of(id, holidays, weekendDays);
		}
		else
		{
		  int weekends = builder.get(WEEKENDS);
		  int startYear = builder.get(START_YEAR);
		  int[] lookup = builder.get(LOOKUP);
		  return new ImmutableHolidayCalendar(id, weekends, startYear, lookup, false);
		}
	  }

	}

}