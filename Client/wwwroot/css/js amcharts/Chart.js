function GeneratePieChart(UlaznaLista) {

    //naziv je bitan samo u mom slucaju ce biti ListaPreporucilaca
    // Themes begin
    am4core.useTheme(am4themes_dataviz);
    am4core.useTheme(am4themes_animated);


    var chart = am4core.create("chartdiv", am4charts.PieChart3D);
    chart.hiddenState.properties.opacity = 1; // this creates initial fade-in

    chart.legend = new am4charts.Legend();
    chart.innerRadius = am4core.percent(40); // ovo sam dodao

    chart.data = UlaznaLista;

    var series = chart.series.push(new am4charts.PieSeries3D());
    series.dataFields.value = "ratio";
    series.dataFields.category = "name";
    series.slices.template.stroke = am4core.color("#fff"); //ovo sam dodao
    series.slices.template.strokeOpacity = 1; //ovo sam dodao
    series.hiddenState.properties.endAngle = -90; //ovo sam dodao
    series.hiddenState.properties.startAngle = -90; //ovo sam dodao
    series.labels.template.text = "{category}: {value.value}";

}