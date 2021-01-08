function GenerisiVariableheight3DPieChart(UlaznaLista) {

    // Themes begin
    am4core.useTheme(am4themes_animated);
    // Themes end

    var chart = am4core.create("chartdiv1", am4charts.PieChart3D);
    chart.hiddenState.properties.opacity = 0; // this creates initial fade-in

    chart.data = UlaznaLista;

    chart.innerRadius = am4core.percent(40);
    chart.depth = 120;

    chart.legend = new am4charts.Legend();

    var series = chart.series.push(new am4charts.PieSeries3D());
    series.dataFields.value = "total";
    series.dataFields.depthValue = "total";
    series.dataFields.category = "name";
    series.slices.template.cornerRadius = 5;
    series.colors.step = 3;
}