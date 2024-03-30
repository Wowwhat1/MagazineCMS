$(document).ready(function () {
    loadChartData();
    loadChartDataUser();
    calculateTopicCounts();
});

function loadChartData() {
    $.ajax({
        url: '/manager/faculty/getall',
        type: 'GET',
        success: function (response) {
            const data = response.data;
            const facultyNames = [];
            const userCounts = [];
            const magazineCounts = [];
            const userInfo = [];

            data.forEach(function (item) {
                facultyNames.push(item.faculty.name);
                userCounts.push(item.userCount);
                magazineCounts.push(item.magazineCount);
                userInfo.push(item.userInfo);
            });


            createUserChart(facultyNames, userCounts);
            createMagazineChart(facultyNames, magazineCounts);
        },
        error: function (xhr, textStatus, errorThrown) {
            console.error("Error fetching faculty data:", errorThrown);
        }
    });
}

function createUserChart(labels, data) {
    var ctx = document.getElementById('userChart').getContext('2d');
    var chart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [{
                label: 'Number of Users',
                data: data,
                backgroundColor: 'rgba(255, 99, 132, 0.2)',
                borderColor: 'rgba(255, 99, 132, 1)',
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true,
                        stepSize: 1
                    }
                }]
            }
        }
    });
}

function createMagazineChart(labels, data) {
    var ctx = document.getElementById('magazineChart').getContext('2d');
    var chart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [{
                label: 'Number of Magazines',
                data: data,
                backgroundColor: 'rgba(54, 162, 235, 0.2)',
                borderColor: 'rgba(54, 162, 235, 1)',
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true,
                        stepSize: 1
                    }
                }]
            }
        }
    });
}

function loadChartDataUser() {
    $.ajax({
        url: '/admin/manageuser/getall',
        type: 'GET',
        success: function (response) {
            const data = response.data;
            const roles = [];
            const userCountsByRole = [];

            data.forEach(function (user) {
                const role = user.role;
                if (!roles.includes(role)) {
                    roles.push(role);
                    userCountsByRole.push(1);
                } else {
                    const index = roles.indexOf(role);
                    userCountsByRole[index]++;
                }
            });

            createRoleChart(roles, userCountsByRole);
        },
        error: function (xhr, textStatus, errorThrown) {
            console.error("Error fetching user data:", errorThrown);
        }
    });
}


function createRoleChart(labels, data) {
    var ctx = document.getElementById('polarUserChart').getContext('2d');
    var chart = new Chart(ctx, {
        type: 'pie',
        data: {
            labels: labels,
            datasets: [{
                label: 'Number of Users',
                data: data,
                backgroundColor: [
                    'rgba(255, 99, 132, 0.2)',
                    'rgba(54, 162, 235, 0.2)',
                    'rgba(255, 206, 86, 0.2)',
                    'rgba(75, 192, 192, 0.2)',
                    'rgba(153, 102, 255, 0.2)',
                    'rgba(255, 159, 64, 0.2)'
                ],
                borderColor: [
                    'rgba(255, 99, 132, 1)',
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 206, 86, 1)',
                    'rgba(75, 192, 192, 1)',
                    'rgba(153, 102, 255, 1)',
                    'rgba(255, 159, 64, 1)'
                ],
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true
                    }
                }]
            },
            tooltips: {
                callbacks: {
                    label: function (tooltipItem, data) {
                        var label = data.labels[tooltipItem.index] || '';
                        var value = data.datasets[0].data[tooltipItem.index];
                        return label + ': ' + value;
                    }
                }
            }
        }
    });
}

var monthNames = [
    "January", "February", "March", "April", "May", "June", "July",
    "August", "September", "October", "November", "December"
];

$(document).ready(function () {

    $.ajax({
        url: '/manager/managemagazine/getall',
        type: 'GET',
        success: function (response) {
            var data = response.data;

            var endDates = [];

            data.forEach(function (topic) {
                endDates.push(new Date(topic.endDate));
            });

            var topicCounts = calculateTopicCounts(endDates);

            var months = Array.from({ length: 12 }, (_, i) => i + 1);

            drawLineChart(months, topicCounts);
        },
        error: function (xhr, textStatus, errorThrown) {
            console.error("Error fetching data:", errorThrown);
        }
    });
});


function calculateTopicCounts(endDates) {
    var topicCounts = Array.from({ length: 12 }, () => 0);

    endDates.forEach(function (endDate) {
        var month = endDate.getMonth();
        topicCounts[month]++;
    });
    return topicCounts;
}


function drawLineChart(months, topicCounts) {
    var ctx = document.getElementById('topicChart').getContext('2d');
    var topicChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: months.map(month => monthNames[month - 1]), 
            datasets: [{
                label: 'Topic Counts by Month',
                data: topicCounts,
                fill: false,
                borderColor: 'rgb(75, 192, 192)',
                tension: 0.1
            }]
        },
        options: {
            scales: {
                x: {
                    title: {
                        display: true,
                        text: 'Month'
                    }
                },
                y: {
                    title: {
                        display: true,
                        text: 'Number of Topics'
                    },
                    beginAtZero: true
                }
            }
        }
    });
}