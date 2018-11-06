using System;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.examples.gui
{

	using Bean = org.joda.beans.Bean;
	using MetaProperty = org.joda.beans.MetaProperty;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using BusinessDayConvention = com.opengamma.strata.basics.date.BusinessDayConvention;
	using BusinessDayConventions = com.opengamma.strata.basics.date.BusinessDayConventions;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using HolidayCalendarIds = com.opengamma.strata.basics.date.HolidayCalendarIds;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using RollConvention = com.opengamma.strata.basics.schedule.RollConvention;
	using RollConventions = com.opengamma.strata.basics.schedule.RollConventions;
	using Schedule = com.opengamma.strata.basics.schedule.Schedule;
	using ScheduleException = com.opengamma.strata.basics.schedule.ScheduleException;
	using SchedulePeriod = com.opengamma.strata.basics.schedule.SchedulePeriod;
	using StubConvention = com.opengamma.strata.basics.schedule.StubConvention;

	using Application = javafx.application.Application;
	using ReadOnlyObjectWrapper = javafx.beans.property.ReadOnlyObjectWrapper;
	using ObservableValue = javafx.beans.value.ObservableValue;
	using FXCollections = javafx.collections.FXCollections;
	using ObservableList = javafx.collections.ObservableList;
	using Insets = javafx.geometry.Insets;
	using Scene = javafx.scene.Scene;
	using Button = javafx.scene.control.Button;
	using ChoiceBox = javafx.scene.control.ChoiceBox;
	using DatePicker = javafx.scene.control.DatePicker;
	using Label = javafx.scene.control.Label;
	using TableColumn = javafx.scene.control.TableColumn;
	using CellDataFeatures = javafx.scene.control.TableColumn.CellDataFeatures;
	using TableView = javafx.scene.control.TableView;
	using BorderPane = javafx.scene.layout.BorderPane;
	using GridPane = javafx.scene.layout.GridPane;
	using Stage = javafx.stage.Stage;
	using Callback = javafx.util.Callback;

	/// <summary>
	/// A simple GUI demonstration of schedule generation.
	/// <para>
	/// This provides a GUI based on <seealso cref="PeriodicSchedule"/> and <seealso cref="Schedule"/>.
	/// </para>
	/// <para>
	/// This GUI exists for demonstration purposes to aid with understanding schedule generation.
	/// It is not intended to be used in a production environment.
	/// </para>
	/// </summary>
	public class ScheduleGui : Application
	{

	  // the reference data to use
	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  /// <summary>
	  /// Launch GUI, no arguments needed.
	  /// </summary>
	  /// <param name="args">  no arguments needed </param>
	  public static void Main(string[] args)
	  {
		launch(args);
	  }

	  //-------------------------------------------------------------------------
	  public override void start(Stage primaryStage)
	  {
		LocalDate today = LocalDate.now(ZoneId.systemDefault());

		// setup GUI elements
		Label startLbl = new Label("Start date:");
		DatePicker startInp = new DatePicker(today);
		startLbl.LabelFor = startInp;
		startInp.ShowWeekNumbers = false;

		Label endLbl = new Label("End date:");
		DatePicker endInp = new DatePicker(today.plusYears(1));
		endLbl.LabelFor = endInp;
		endInp.ShowWeekNumbers = false;

		Label freqLbl = new Label("Frequency:");
		ChoiceBox<Frequency> freqInp = new ChoiceBox<Frequency>(FXCollections.observableArrayList(Frequency.P1M, Frequency.P2M, Frequency.P3M, Frequency.P4M, Frequency.P6M, Frequency.P12M));
		freqLbl.LabelFor = freqInp;
		freqInp.Value = Frequency.P3M;

		Label stubLbl = new Label("Stub:");
		ObservableList<StubConvention> stubOptions = FXCollections.observableArrayList(StubConvention.values());
		stubOptions.add(0, null);
		ChoiceBox<StubConvention> stubInp = new ChoiceBox<StubConvention>(stubOptions);
		stubLbl.LabelFor = stubInp;
		stubInp.Value = StubConvention.SMART_INITIAL;

		Label rollLbl = new Label("Roll:");
		ChoiceBox<RollConvention> rollInp = new ChoiceBox<RollConvention>(FXCollections.observableArrayList(null, RollConventions.NONE, RollConventions.EOM, RollConventions.IMM, RollConventions.IMMAUD, RollConventions.IMMNZD, RollConventions.SFE));
		rollLbl.LabelFor = rollInp;
		rollInp.Value = RollConventions.NONE;

		Label bdcLbl = new Label("Adjust:");
		ChoiceBox<BusinessDayConvention> bdcInp = new ChoiceBox<BusinessDayConvention>(FXCollections.observableArrayList(BusinessDayConventions.NO_ADJUST, BusinessDayConventions.FOLLOWING, BusinessDayConventions.MODIFIED_FOLLOWING, BusinessDayConventions.PRECEDING, BusinessDayConventions.MODIFIED_PRECEDING, BusinessDayConventions.MODIFIED_FOLLOWING_BI_MONTHLY, BusinessDayConventions.NEAREST));
		bdcLbl.LabelFor = bdcInp;
		bdcInp.Value = BusinessDayConventions.MODIFIED_FOLLOWING;

		Label holidayLbl = new Label("Holidays:");
		ChoiceBox<HolidayCalendarId> holidayInp = new ChoiceBox<HolidayCalendarId>(FXCollections.observableArrayList(HolidayCalendarIds.CHZU, HolidayCalendarIds.GBLO, HolidayCalendarIds.EUTA, HolidayCalendarIds.FRPA, HolidayCalendarIds.JPTO, HolidayCalendarIds.NYFD, HolidayCalendarIds.NYSE, HolidayCalendarIds.USNY, HolidayCalendarIds.USGS, HolidayCalendarIds.NO_HOLIDAYS, HolidayCalendarIds.SAT_SUN));
		holidayLbl.LabelFor = holidayInp;
		holidayInp.Value = HolidayCalendarIds.GBLO;

		TableView<SchedulePeriod> resultGrid = new TableView<SchedulePeriod>();
		TableColumn<SchedulePeriod, LocalDate> unadjustedCol = new TableColumn<SchedulePeriod, LocalDate>("Unadjusted dates");
		TableColumn<SchedulePeriod, LocalDate> adjustedCol = new TableColumn<SchedulePeriod, LocalDate>("Adjusted dates");

		TableColumn<SchedulePeriod, LocalDate> resultUnadjStartCol = new TableColumn<SchedulePeriod, LocalDate>("Start");
		resultUnadjStartCol.CellValueFactory = new TableCallback<>(SchedulePeriod.meta().unadjustedStartDate());
		TableColumn<SchedulePeriod, LocalDate> resultUnadjEndCol = new TableColumn<SchedulePeriod, LocalDate>("End");
		resultUnadjEndCol.CellValueFactory = new TableCallback<>(SchedulePeriod.meta().unadjustedEndDate());
		TableColumn<SchedulePeriod, Period> resultUnadjLenCol = new TableColumn<SchedulePeriod, Period>("Length");
		resultUnadjLenCol.CellValueFactory = ReadOnlyCallback.of(sch => Period.between(sch.UnadjustedStartDate, sch.UnadjustedEndDate));

		TableColumn<SchedulePeriod, LocalDate> resultStartCol = new TableColumn<SchedulePeriod, LocalDate>("Start");
		resultStartCol.CellValueFactory = new TableCallback<>(SchedulePeriod.meta().startDate());
		TableColumn<SchedulePeriod, LocalDate> resultEndCol = new TableColumn<SchedulePeriod, LocalDate>("End");
		resultEndCol.CellValueFactory = new TableCallback<>(SchedulePeriod.meta().endDate());
		TableColumn<SchedulePeriod, Period> resultLenCol = new TableColumn<SchedulePeriod, Period>("Length");
		resultLenCol.CellValueFactory = ReadOnlyCallback.of(sch => sch.length());

		unadjustedCol.Columns.add(resultUnadjStartCol);
		unadjustedCol.Columns.add(resultUnadjEndCol);
		unadjustedCol.Columns.add(resultUnadjLenCol);
		adjustedCol.Columns.add(resultStartCol);
		adjustedCol.Columns.add(resultEndCol);
		adjustedCol.Columns.add(resultLenCol);
		resultGrid.Columns.add(unadjustedCol);
		resultGrid.Columns.add(adjustedCol);
		resultGrid.Placeholder = new Label("Schedule not yet generated");

		unadjustedCol.prefWidthProperty().bind(resultGrid.widthProperty().divide(2));
		adjustedCol.prefWidthProperty().bind(resultGrid.widthProperty().divide(2));
		resultUnadjStartCol.prefWidthProperty().bind(unadjustedCol.widthProperty().divide(3));
		resultUnadjEndCol.prefWidthProperty().bind(unadjustedCol.widthProperty().divide(3));
		resultUnadjLenCol.prefWidthProperty().bind(unadjustedCol.widthProperty().divide(3));
		resultStartCol.prefWidthProperty().bind(adjustedCol.widthProperty().divide(3));
		resultEndCol.prefWidthProperty().bind(adjustedCol.widthProperty().divide(3));
		resultLenCol.prefWidthProperty().bind(adjustedCol.widthProperty().divide(3));

		// setup generation button
		// this uses the GUI thread which is not the best idea
		Button btn = new Button();
		btn.Text = "Generate";
		btn.OnAction = @event =>
		{
		LocalDate start = startInp.Value;
		LocalDate end = endInp.Value;
		Frequency freq = freqInp.Value;
		StubConvention stub = stubInp.Value;
		RollConvention roll = rollInp.Value;
		HolidayCalendarId holCal = holidayInp.Value;
		BusinessDayConvention bdc = bdcInp.Value;
		BusinessDayAdjustment bda = BusinessDayAdjustment.of(bdc, holCal);
		PeriodicSchedule defn = PeriodicSchedule.builder().startDate(start).endDate(end).frequency(freq).businessDayAdjustment(bda).stubConvention(stub).rollConvention(roll).build();
		try
		{
			Schedule schedule = defn.createSchedule(REF_DATA);
			Console.WriteLine(schedule);
			resultGrid.Items = FXCollections.observableArrayList(schedule.Periods);
		}
		catch (ScheduleException ex)
		{
			resultGrid.Items = FXCollections.emptyObservableList();
			resultGrid.Placeholder = new Label(ex.Message);
			Console.WriteLine(ex.Message);
		}
		};

		// layout the components
		GridPane gp = new GridPane();
		gp.Hgap = 10;
		gp.Vgap = 10;
		gp.Padding = new Insets(0, 10, 0, 10);
		gp.add(startLbl, 1, 1);
		gp.add(startInp, 2, 1);
		gp.add(endLbl, 1, 2);
		gp.add(endInp, 2, 2);
		gp.add(freqLbl, 1, 3);
		gp.add(freqInp, 2, 3);
		gp.add(bdcLbl, 3, 1);
		gp.add(bdcInp, 4, 1);
		gp.add(holidayLbl, 3, 2);
		gp.add(holidayInp, 4, 2);
		gp.add(stubLbl, 3, 3);
		gp.add(stubInp, 4, 3);
		gp.add(rollLbl, 3, 4);
		gp.add(rollInp, 4, 4);
		gp.add(btn, 3, 5, 2, 1);
		gp.add(resultGrid, 1, 7, 4, 1);

		BorderPane bp = new BorderPane(gp);
		Scene scene = new Scene(bp, 600, 600);

		// launch
		primaryStage.Title = "Periodic schedule generator";
		primaryStage.Scene = scene;
		primaryStage.show();
	  }

	  //-------------------------------------------------------------------------
	  // link Joda-Bean meta property to JavaFX
	  internal class TableCallback<S, T> : Callback<TableColumn.CellDataFeatures<S, T>, ObservableValue<T>> where S : org.joda.beans.Bean
	  {
		internal readonly MetaProperty<T> property;

		public TableCallback(MetaProperty<T> property)
		{
		  this.property = property;
		}

		public override ObservableValue<T> call(TableColumn.CellDataFeatures<S, T> param)
		{
		  return getCellDataReflectively(param.Value);
		}

		internal virtual ObservableValue<T> getCellDataReflectively(S rowData)
		{
		  if (property == null || rowData == null)
		  {
			return null;
		  }
		  T value = property.get(rowData);
		  if (value == default(T))
		  {
			return null;
		  }
		  return new ReadOnlyObjectWrapper<T>(value);
		}
	  }

	  // allow simpler way to define a callback
	  internal interface ReadOnlyCallback<S, T> : Callback<TableColumn.CellDataFeatures<S, T>, ObservableValue<T>>
	  {

//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//		public static <S, T> javafx.util.Callback<javafx.scene.control.TableColumn.CellDataFeatures<S, T>, javafx.beans.value.ObservableValue<T>> of(ReadOnlyCallback<S, T> underlying)
	//	{
	//	  return underlying;
	//	}

//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//		public default javafx.beans.value.ObservableValue<T> call(javafx.scene.control.TableColumn.CellDataFeatures<S, T> param)
	//	{
	//	  return new ReadOnlyObjectWrapper<T>(callValue(param.getValue()));
	//	}

		T callValue(S value);
	  }

	}

}