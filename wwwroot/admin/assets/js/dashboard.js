(function ($) {
  'use strict';
    if ($("#visit-sale-chart").length) {
      const ctx = document.getElementById('visit-sale-chart');

      var graphGradient1 = ctx.getContext("2d");
      var graphGradient2 = ctx.getContext("2d");

      var gradientStrokeViolet = graphGradient1.createLinearGradient(0, 0, 0, 181);
      gradientStrokeViolet.addColorStop(0, 'rgba(218, 140, 255, 1)');
      gradientStrokeViolet.addColorStop(1, 'rgba(154, 85, 255, 1)');
      var gradientLegendViolet = 'linear-gradient(to right, rgba(218, 140, 255, 1), rgba(154, 85, 255, 1))';

      var gradientStrokeBlue = graphGradient2.createLinearGradient(0, 0, 0, 360);
      gradientStrokeBlue.addColorStop(0, 'rgba(54, 215, 232, 1)');
      gradientStrokeBlue.addColorStop(1, 'rgba(177, 148, 250, 1)');
      var gradientLegendBlue = 'linear-gradient(to right, rgba(54, 215, 232, 1), rgba(177, 148, 250, 1))';

      const bgColor1 = ["rgba(218, 140, 255, 1)"];
      const bgColor2 = ["rgba(54, 215, 232, 1)"];

      new Chart(ctx, {
          type: 'bar',
          data: {
              labels: window.chartData.monthLabels,
              datasets: [{
                      label: "Người dùng",
                      borderColor: gradientStrokeViolet,
                      backgroundColor: gradientStrokeViolet,
                      fillColor: bgColor1,
                      hoverBackgroundColor: gradientStrokeViolet,
                      pointRadius: 0,
                      fill: false,
                      borderWidth: 1,
                      fill: 'origin',
                      data: window.chartData.userCounts,
                      barPercentage: 0.5,
                      categoryPercentage: 0.5,
                  },
                  {
                      label: "Lượt thuê sách",
                      borderColor: gradientStrokeBlue,
                      backgroundColor: gradientStrokeBlue,
                      hoverBackgroundColor: gradientStrokeBlue,
                      fillColor: bgColor2,
                      pointRadius: 0,
                      fill: false,
                      borderWidth: 1,
                      fill: 'origin',
                      data: window.chartData.rentalCounts,
                      barPercentage: 0.5,
                      categoryPercentage: 0.5,
                  }
              ]
          },
          options: {
              responsive: true,
              maintainAspectRatio: true,
              elements: {
                  line: {
                      tension: 0.4,
                  },
              },
              scales: {
                  y: {
                      display: false,
                      grid: {
                          display: true,
                          drawOnChartArea: true,
                          drawTicks: false,
                      },
                  },
                  x: {
                      display: true,
                      grid: {
                          display: false,
                      },
                  }
              },
              plugins: {
                  legend: {
                      display: false,
                  }
              }
          },
          plugins: [{
              afterDatasetUpdate: function(chart, args, options) {
                  const chartId = chart.canvas.id;
                  const legendId = `${chartId}-legend`;
                  const ul = document.createElement('ul');
                  for (let i = 0; i < chart.data.datasets.length; i++) {
                      ul.innerHTML += `
                          <li>
                              <span style="background-color: ${chart.data.datasets[i].fillColor}"></span>
                              ${chart.data.datasets[i].label}
                          </li>
                      `;
                  }
                  return document.getElementById(legendId).appendChild(ul);
              }
          }]
      });
  }

  if ($("#traffic-chart").length) {
    const ctx = document.getElementById('traffic-chart');

    var graphGradient1 = document.getElementById("traffic-chart").getContext('2d');
    var graphGradient2 = document.getElementById("traffic-chart").getContext('2d');

    // Gradient xanh
    var gradientStrokeBlue = graphGradient1.createLinearGradient(0, 0, 0, 181);
    gradientStrokeBlue.addColorStop(0, 'rgba(54, 215, 232, 1)');
    gradientStrokeBlue.addColorStop(1, 'rgba(177, 148, 250, 1)');
    var gradientLegendBlue = 'rgba(54, 215, 232, 1)';

    // Gradient vàng
    var gradientStrokeYellow = graphGradient2.createLinearGradient(0, 0, 0, 181);
    gradientStrokeYellow.addColorStop(0, 'rgba(255, 235, 59, 1)');
    gradientStrokeYellow.addColorStop(1, 'rgba(255, 193, 7, 1)');
    var gradientLegendYellow = 'rgba(255, 193, 7, 1)';

    new Chart(ctx, {
      type: 'doughnut',
      data: {
        labels: ['Đã hoàn thành', 'Đang cập nhật'],
        datasets: [{
          data: [
            window.chartData.completedBooks,
            window.chartData.inProgressBooks,
          ],
          backgroundColor: [gradientStrokeBlue, gradientStrokeYellow],
          hoverBackgroundColor: [
            gradientStrokeBlue,
            gradientStrokeYellow
          ],
          borderColor: [
            gradientStrokeBlue,
            gradientStrokeYellow
          ],
          legendColor: [
            gradientLegendBlue,
            gradientLegendYellow
          ]
        }]
      },
      options: {
        cutout: 50,
        animationEasing: "easeOutBounce",
        animateRotate: true,
        animateScale: false,
        responsive: true,
        maintainAspectRatio: true,
        showScale: true,
        legend: false,
        plugins: {
          legend: {
            display: false,
          }
        }
      },
      plugins: [{
        afterDatasetUpdate: function (chart, args, options) {
          const chartId = chart.canvas.id;
          var i;
          const legendId = `${chartId}-legend`;
          const ul = document.createElement('ul');
          for (i = 0; i < chart.data.datasets[0].data.length; i++) {
            ul.innerHTML += `
                <li>
                  <span style="background-color: ${chart.data.datasets[0].legendColor[i]}"></span>
                  ${chart.data.labels[i]}
                </li>
              `;
          }
          return document.getElementById(legendId).appendChild(ul);
        }
      }]
    });
  }



  if ($("#inline-datepicker").length) {
    $('#inline-datepicker').datepicker({
      enableOnReadonly: true,
      todayHighlight: true,
    });
  }
  if ($.cookie('purple-pro-banner') != "true") {
    document.querySelector('#proBanner').classList.add('d-flex');
    document.querySelector('.navbar').classList.remove('fixed-top');
  } else {
    document.querySelector('#proBanner').classList.add('d-none');
    document.querySelector('.navbar').classList.add('fixed-top');
  }

  if ($(".navbar").hasClass("fixed-top")) {
    document.querySelector('.page-body-wrapper').classList.remove('pt-0');
    document.querySelector('.navbar').classList.remove('pt-5');
  } else {
    document.querySelector('.page-body-wrapper').classList.add('pt-0');
    document.querySelector('.navbar').classList.add('pt-5');
    document.querySelector('.navbar').classList.add('mt-3');

  }
  document.querySelector('#bannerClose').addEventListener('click', function () {
    document.querySelector('#proBanner').classList.add('d-none');
    document.querySelector('#proBanner').classList.remove('d-flex');
    document.querySelector('.navbar').classList.remove('pt-5');
    document.querySelector('.navbar').classList.add('fixed-top');
    document.querySelector('.page-body-wrapper').classList.add('proBanner-padding-top');
    document.querySelector('.navbar').classList.remove('mt-3');
    var date = new Date();
    date.setTime(date.getTime() + 24 * 60 * 60 * 1000);
    $.cookie('purple-pro-banner', "true", {
      expires: date
    });
  });
})(jQuery);