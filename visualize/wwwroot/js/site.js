// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(() => {

    //Replaces the placeholder in file selectors with the file names
    $('#datasetFile,#testDataset').each(function () {
        $(this).on('change', function () {
            //get the file name
            var fileName = $(this).val();
            var split = fileName.split('\\').join('/').split('/');
            //replace the "Choose a file" label
            $(this).next('.custom-file-label').html(split[split.length - 1]);
        });
    });

    if (document.getElementById('chartsIndex')) {
        //Handle ajax request for histograms
        $("#histogramCourseSelector").on("change", () => {
            var option = $("#histogramCourseSelector").find(":selected").text();
            if (option != "Select a course") {
                $.ajax(
                    {
                        type: "POST",
                        url: "GenerateHistogramDataPost",
                        data: { courseName: option.toString() },
                        success: displayHistogram,
                        error: displayHistogramError
                    }
                )
            }
        })
      
        //Handle scatter plot select boxes
        $("#scatterCourse1Selector").on("change", () => {
            handleScatterPlot()
        })
        $("#scatterCourse2Selector").on("change", () => {
            handleScatterPlot()
        })

        //Handle pairplot redirection
        $("#pairplot-tab").on("click", () => {
            window.location.href = "/Charts/PairPlot"
        })
    }

    //Handle pairplot creation
    if (document.getElementById('pairPlotTable')) {
        $("#legend").hide();
        createPairPlot()
    }

    //Handle training results datatable
    if (document.getElementById('trainIndex')) {
        $("#weightsDatatable").DataTable({
            "paging": false,
            "search": false,
            "ordering": false,
            "searching": false,
            "bInfo": false,
            "info": false
        });
    }

    //Handle prediction result datatable
    if (document.getElementById('predictIndex')) {
        $('#predictionDatatable').DataTable();
    }

    //Handle modal display when submitting training form
    $("#trainForm").on("submit", () => {
        $("#trainingModal").modal({ backdrop: 'static' }).modal('show');
    })
})


function handleScatterPlot() {
    var option1 = $("#scatterCourse1Selector").find(":selected").text();
    var option2 = $("#scatterCourse2Selector").find(":selected").text();

    //here we are disabling the selected options in the opposite selectbox, because we can't select two times the same course
    //we first enable all options back to avoid locking already selected ones forever
    $("#scatterCourse1Selector").find(":disabled").prop("disabled", false)
    $("#scatterCourse2Selector").find(":disabled").prop("disabled", false)
    //we disable back the "Select a course" options back
    $("#scatterCourse1Selector").children().first().prop("disabled", true)
    $("#scatterCourse2Selector").children().first().prop("disabled", true)
    //we disable the selected ones in the opposite box
    $("#scatterCourse2Selector").find("option:contains('" + option1 + "')").prop("disabled", true)
    $("#scatterCourse1Selector").find("option:contains('" + option2 + "')").prop("disabled", true)

    if (option1 != option2 && option1 != "Select a course" && option2 != "Select a course") {
        $.ajax(
            {
                type: "POST",
                url: "GenerateScatterPlotDataPost",
                data: { courseName1: option1.toString(), courseName2: option2.toString() },
                success: displayScatterPlot,
                error: displayScatterPlotError
            }
        )
    }
}

function displayHistogram(data) {
    if (data == false)
        displayHistogramError()
    else {
        var chart = Highcharts.chart('histogramChart', {
            title: {
                text: data['title']
            },
            subtitle: {
                text: data['subtitle']
            },
            xAxis: [{
                opposite: true,
                visible: false
            }, {
                title: { text: data['xAxisName'] }
            }],

            yAxis: [{
                opposite: true,
                visible: false
            }, {
                title: { text: data['yAxisName'] }
            }],

            plotOptions: {
                series: {
                    turboThreshold: 10000
                },
                histogram: {
                    accessibility: {
                        point: {
                            valueDescriptionFormat: '{index}. {point.x:.3f} to {point.x2:.3f}, {point.y}.'
                        }
                    },
                }
            },
            credits: {
                enabled: false
            },
            series: [
                //Gryffindor
                {
                    name: data['series'][0]['name'],
                    type: 'histogram',
                    xAxis: 1,
                    color: 'rgba(101,0,0,255)',
                    opacity: 0.6,
                    yAxis: 1,
                    baseSeries: 'gryffindorNotes',
                    zIndex: -1
                },
                {
                    name: 'gData',
                    type: 'scatter',
                    data: data['series'][0]['data'],
                    visible: false,
                    id: 'gryffindorNotes',
                },
                //Hufflepuff
                {
                    name: data['series'][1]['name'],
                    type: 'histogram',
                    xAxis: 1,
                    color: 'rgba(255,157,10,255)',
                    opacity: 0.6,
                    yAxis: 1,
                    baseSeries: 'hufflepuffNotes',
                    zIndex: -1
                }, {
                    name: 'hData',
                    type: 'scatter',
                    data: data['series'][1]['data'],
                    visible: false,
                    id: 'hufflepuffNotes',
                },
                //Slytherin
                {
                    name: data['series'][2]['name'],
                    type: 'histogram',
                    xAxis: 1,
                    color: 'rgba(47,117,28,255)',
                    opacity: 0.6,
                    yAxis: 1,
                    baseSeries: 'slytherinNotes',
                    zIndex: -1
                }, {
                    name: 'sData',
                    type: 'scatter',
                    data: data['series'][2]['data'],
                    visible: false,
                    id: 'slytherinNotes',
                },
                //Ravenclaw
                {
                    name: data['series'][3]['name'],
                    type: 'histogram',
                    xAxis: 1,
                    color: 'rgba(26,57,86,255)',
                    opacity: 0.6,
                    yAxis: 1,
                    baseSeries: 'ravenclawNotes',
                    zIndex: -1
                }, {
                    name: 'rData',
                    type: 'scatter',
                    data: data['series'][3]['data'],
                    visible: false,
                    id: 'ravenclawNotes',
                }
            ]
        });
        //remove the scatter series from the chart since we don't need them
        chart.get('ravenclawNotes').remove();
        chart.get('slytherinNotes').remove();
        chart.get('hufflepuffNotes').remove();
        chart.get('gryffindorNotes').remove();
    }
}

function displayHistogramError() {
    $("#histogramChart").html("<div class='alert alert-danger' role='alert'>An errror occurred while requesting the selected course histogram.</div>")
}

function displayScatterPlot(data) {
    if (data == false)
        displayScatterPlotError()
    else {
        Highcharts.chart('scatterChart', {
            chart: {
                type: 'scatter',
                zoomType: 'xy'
            },
            plotOptions:
            {
                scatter: {
                    marker: {
                        radius: 5,
                        states: {
                            hover: {
                                enabled: true,
                                lineColor: 'rgb(100,100,100)'
                            }
                        },
                    }
                },
                series: {
                    turboThreshold: 10000
                },

            },
            title: {
                text: data['title']
            },
            subtitle: {
                text: data['subtitle']
            },
            tooltip: {
                headerFormat: '<b>{series.name}</b><br>',
                pointFormat: "<b>" + data['xAxisName'] + ": </b>{point.x}<br><b>" + data['yAxisName'] + ": </b>{point.y}"
            },
            xAxis: {
                title: {
                    enabled: true,
                    text: data['xAxisName']
                },
                crosshair: true,
                startOnTick: true,
                endOnTick: true,
                showLastLabel: true
            },
            yAxis: {
                title: {
                    text: data['yAxisName']
                },
                crosshair: true,
                startOnTick: true,
                endOnTick: true,
                showLastLabel: true
            },
            credits: {
                enabled: false
            },
            series: [
                {
                    //Gryffindor
                    name: data['series'][0]['name'],
                    color: 'rgba(101,0,0,255)',
                    data: data['series'][0]['data']
                },
                {
                    //Hufflepuff
                    name: data['series'][1]['name'],
                    color: 'rgba(255,157,10,255)',
                    data: data['series'][1]['data']
                },
                {
                    //Slytherin
                    name: data['series'][2]['name'],
                    color: 'rgba(47,117,28,255)',
                    data: data['series'][2]['data']
                },
                {
                    //Ravenclaw
                    name: data['series'][3]['name'],
                    color: 'rgba(26,57,86,255)',
                    data: data['series'][3]['data']
                }
            ]
        })
    }
}

function displayScatterPlotError() {
    $("#scatterChart").html("<div class='alert alert-danger' role='alert'>An errror occurred while requesting the selected courses scatter plot.</div>")
}

function createPairPlot() {
    $.ajax(
        {
            type: "POST",
            url: "GeneratePairPlotData",
            data: {},
            success: displayPairPlot,
            error: displayPairPlotError
        }
    )
}

function displayPairPlot(data) {
    $("#loadingPairPlot").hide();
    $("#spinnerPairPlot").hide();
    $("#legend").show();
    for (var y = 0; y < 13; ++y) {
        for (var x = 0; x < 13; ++x) {
            if (x == y)
                createPairPlotHistogramChart(data[13 * y + x])
            else
                createPairPlotScatterChart(data[13 * y + x])
        }
    }
}

function createPairPlotHistogramChart(data) {
    var chart = Highcharts.chart(data['id'], {
        chart: {
            type: 'histogram',
            height: 150,
            width: 250
        },
        title: {
            text: data['title']
        },
        subtitle: {
            text: data['subtitle']
        },

        xAxis: [{
            opposite: true,
            visible: false
        }, {
            title: { text: data['xAxisName'] }
        }],

        yAxis: [{
            opposite: true,
            visible: false
        }, {
            title: { text: data['yAxisName'] }
        }],

        plotOptions: {
            histogram: {
                marker: {
                    states: {
                        hover: {
                            enabled: false,
                            lineColor: 'rgb(100,100,100)'
                        }
                    },
                },
            },
            series: {
                states: {
                    inactive: {
                        opacity: 1
                    }
                },
                events: {
                    legendItemClick: function () {
                        return false;
                    }
                }
            }
        },
        credits: {
            enabled: false
        },
        series: [
            //Gryffindor
            {
                name: data['series'][0]['name'],
                type: 'histogram',
                xAxis: 1,
                color: 'rgba(101,0,0,255)',
                opacity: 0.6,
                yAxis: 1,
                enableMouseTracking: false,
                "showInLegend": false,
                baseSeries: 'gryffindorNotes',
                zIndex: -1
            },
            {
                name: 'gData',
                type: 'scatter',
                data: data['series'][0]['data'],
                visible: false,
                id: 'gryffindorNotes',
            },
            //Hufflepuff
            {
                name: data['series'][1]['name'],
                type: 'histogram',
                xAxis: 1,
                color: 'rgba(255,157,10,255)',
                opacity: 0.6,
                yAxis: 1,
                enableMouseTracking: false,
                baseSeries: 'hufflepuffNotes',
                "showInLegend": false,
                zIndex: -1
            }, {
                name: 'hData',
                type: 'scatter',
                data: data['series'][1]['data'],
                visible: false,
                id: 'hufflepuffNotes',
            },
            //Slytherin
            {
                name: data['series'][2]['name'],
                type: 'histogram',
                xAxis: 1,
                color: 'rgba(47,117,28,255)',
                opacity: 0.6,
                enableMouseTracking: false,
                yAxis: 1,
                "showInLegend": false,
                baseSeries: 'slytherinNotes',
                zIndex: -1
            }, {
                name: 'sData',
                type: 'scatter',
                data: data['series'][2]['data'],
                visible: false,
                id: 'slytherinNotes',
            },
            //Ravenclaw
            {
                name: data['series'][3]['name'],
                type: 'histogram',
                xAxis: 1,
                color: 'rgba(26,57,86,255)',
                enableMouseTracking: false,
                opacity: 0.6,
                yAxis: 1,
                baseSeries: 'ravenclawNotes',
                "showInLegend": false,
                zIndex: -1
            }, {
                name: 'rData',
                type: 'scatter',
                data: data['series'][3]['data'],
                visible: false,
                id: 'ravenclawNotes',
            }
        ]
    });
    //remove the scatter series from the chart since we don't need them
    chart.get('ravenclawNotes').remove();
    chart.get('slytherinNotes').remove();
    chart.get('hufflepuffNotes').remove();
    chart.get('gryffindorNotes').remove();
}

function createPairPlotScatterChart(data) {
    Highcharts.chart(data['id'], {
        chart: {
            type: 'scatter',
            height: 150,
            width: 250
        },
        plotOptions:
        {
            scatter: {
                marker: {
                    states: {
                        hover: {
                            enabled: false,
                            lineColor: 'rgb(100,100,100)'
                        }
                    },
                }
            },
            series: {
                turboThreshold: 10000,
                enableMouseTracking: false,
                states: {
                    inactive: {
                        opacity: 1
                    }
                },
                events: {
                    legendItemClick: function () {
                        return false;
                    }
                }
            },
        },
        title: {
            text: data['title']
        },
        subtitle: {
            text: data['subtitle']
        },
        credits: {
            enabled: false
        },
        xAxis: {
            title: {
                enabled: true,
                text: data['xAxisName']
            },
            crosshair: false,
            startOnTick: true,
            endOnTick: true,
            showLastLabel: true
        },
        yAxis: {
            title: {
                text: data['yAxisName']
            },
            crosshair: false,
            startOnTick: true,
            endOnTick: true,
            showLastLabel: true
        },
        series: [
            {
                //Gryffindor
                name: data['series'][0]['name'],
                color: 'rgba(101,0,0,255)',
                opacity: 0.6,
                data: data['series'][0]['data'],
                "showInLegend": false,
            },
            {
                //Hufflepuff
                name: data['series'][1]['name'],
                color: 'rgba(255,157,10,255)',
                opacity: 0.6,
                "showInLegend": false,
                data: data['series'][1]['data']
            },
            {
                //Slytherin
                name: data['series'][2]['name'],
                color: 'rgba(47,117,28,255)',
                opacity: 0.6,
                "showInLegend": false,
                data: data['series'][2]['data']
            },
            {
                //Ravenclaw
                name: data['series'][3]['name'],
                color: 'rgba(26,57,86,255)',
                opacity: 0.6,
                "showInLegend": false,
                data: data['series'][3]['data']
            }
        ]
    })
}

function displayPairPlotError(){
    $("#pairPlotSpinner").hide();
}