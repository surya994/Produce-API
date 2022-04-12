
$(document).ready(function () {
    GenderStat();
    UniversityStat();
})
function GenderStat() {
    $.ajax({
        url: 'https://localhost:44375/api/employee/master',
        success: function (data) {
            var male = 0;
            var female = 0;
            for (var i = 0; i < data.result.length; i++) {
                if (data.result[i].gender == "Male") {
                    male += 1;
                } else if (data.result[i].gender == "Female") {
                    female += 1;
                }
            }
            var options = {
                series: [male, female],
                chart: {
                    type: 'pie',
                    toolbar: {
                        show: true
                    },
                },
                labels: ['Male', 'Female'],
                responsive: [{
                    breakpoint: 480,
                    options: {
                        chart: {
                            width: 100
                        },
                        legend: {
                            position: 'bottom'
                        }
                    }
                }]
            };
            var chart = new ApexCharts(document.querySelector("#chartGender"), options);
            chart.render();
        }
    });
}
function UniversityStat() {
    var univCount = [];
    $.ajax({
        async :false,
        url: 'https://localhost:44375/api/university',
        success: function (data1) {
            $.ajax({
                async: false,
                url: 'https://localhost:44375/api/employee/master',
                success: function (data2) {
                    $.each(data1.result, function (key1, val1) {
                        var count = 0;
                        $.each(data2.result, function (key2, val2) {
                            if (val2.universityName == val1.name) {
                                count += 1;
                            }
                        })
                        univCount.push({ name: val1.name, count: count})
                    })
                }
            });
        }
    });
    var data = [];
    var label = [];
    $.each(univCount, function (key, val) {
        label.push(val.name)
        data.push(val.count)
    })
    var options = {
        series: [{
            data: data
        }],
        chart: {
            type: 'bar',
            height: 200
        },
        plotOptions: {
            bar: {
                borderRadius: 4,
                horizontal: true,
            }
        },
        dataLabels: {
            enabled: true
        },
        xaxis: {
            categories: label,
        }
    };

    var chart = new ApexCharts(document.querySelector("#chartUniversity"), options);
    chart.render();
    
}

