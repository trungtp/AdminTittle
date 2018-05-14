var areaChartOptions = {
    responsive: true,
    legend: {
        position: "right",
    },
    hoverMode: 'index',
    stacked: false,
    title: {
        display: false
    },
    scales: {
        yAxes: [{
            ticks: {
                min: 0
            }
        }]
    }
}

//-------------
//- LINE CHART -
//--------------
$(document).ajaxStart(function () { Pace.restart(); });
$(document).ready(function () {
    var loc = window.location.href;
    $(".sidebar-menu li").removeClass("active");
 if (loc.toLowerCase().indexOf("user-dashboard") >= 0) {
     $(".sidebar-menu a[href='/user-dashboard']").parents('li').addClass("active");
        getUserActionChart();
        getUserPromoCodeChart();
        getUserPlanChart();
    }
 else if (loc.toLowerCase().indexOf("dashboard") >= 0) {
        $(".sidebar-menu a[href='/dashboard']").parent('li').addClass("active");
        //get total users till today
        getTotalUsers();
        //get new users till today
        getNewUsers(0);
        getActiveUsers(0);
        getTerminatedUsers(0);
        getRevenueChart(0);
        getInappPurchases(0);
        getAddonFeatures();
    }
 else if (loc.toLowerCase().indexOf("report") >= 0) {
     $(".sidebar-menu a[href='/report']").parents('li').addClass("active");
        $("#reminder-notification").load("/User/ReminderSetting");
        $("#selectReport").select2({ minimumResultsForSearch: -1 });
        $("#selectFilter").select2();
        //$("#selectFilter option").prop("selected", "selected");
        //$("#selectFilter").trigger("change");
        var start = moment().subtract(29, 'days');
        var end = moment();

        $('#reportDate').daterangepicker({
            "alwaysShowCalendars": true,
            "opens": "left",
            autoUpdateInput: false,
            locale: {
                cancelLabel: 'Clear'
            },
            ranges: {
                'Today': [moment(), moment()],
                'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                'Last 7 Days': [moment().subtract(6, 'days'), moment()],
                'Last 30 Days': [moment().subtract(29, 'days'), moment()],
                'This Month': [moment().startOf('month'), moment().endOf('month')],
                'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
            }
        });
        $('#reportDate').on('apply.daterangepicker', function (ev, picker) {
            $(this).val(picker.startDate.format('MM/DD/YYYY') + ' - ' + picker.endDate.format('MM/DD/YYYY'));
            onReportChange();
        });
        $('#reportDate').on('cancel.daterangepicker', function(ev, picker) {
            $(this).val('');
            onReportChange();
        });
        //cb(start, end);
        $('#selectFilter').on('change', function (e) {
            onReportChange();
        });
        getUsersList();
    }
 else if (loc.toLowerCase().indexOf("promocode") >= 0) {
     $(".sidebar-menu a[href='/promocode']").parents('li').addClass("active");
        getPromoCodesList();
    }
 else if (loc.toLowerCase().indexOf("user-tasks") >= 0) {
     $(".sidebar-menu a[href='/user-tasks']").parent('li').addClass("active");
        getTasksList();
    }
 else if (loc.toLowerCase().indexOf("user-redeems") >= 0) {
     $(".sidebar-menu a[href='/user-redeems']").parents('li').addClass("active");
        getRedeemsList();
    }
 else if (loc.toLowerCase().indexOf("redeems") >= 0) {
     $(".sidebar-menu a[href='/redeems']").parents('li').addClass("active");
        getRedemptionList();
    }
 else if (loc.toLowerCase().indexOf("notification") >= 0) {
     $(".sidebar-menu a[href='/notification']").parent('li').addClass("active");
        getNotificationsList();
    }
 else if (loc.toLowerCase().indexOf("language") >= 0) {
     $(".sidebar-menu a[href='/language']").parent('li').addClass("active");
        getLanguagesList();

        $('#example1').on('change', 'td.editable', function (evt, value) {
            if (value.trim() == '') value = value.trim();
            if ($(this).data('type') == 'key') {
                var url = '/Language/UpdateKey';
            } else if ($(this).data('type') == 'translation') {
                var url = '/Language/UpdateTranslation';
            }

            $.ajax({
                url: url,
                type: 'POST',
                data: {
                    'Id': $(this).data('id'),
                    'value': value
                },
                success: function (data) {
                    if (data.status)
                        showMessage("success", data.message);
                    else
                        showMessage("error", data.message);
                },
                error: function (data) {
                    //console.log(data);
                }
            });
        });
    }
 else if (loc.toLowerCase().indexOf("user-plans") >= 0) {
     $(".sidebar-menu a[href='/user-plans']").parents('li').addClass("active");
        getPlansList();
    }
 else if (loc.toLowerCase().indexOf("user-actions") >= 0) {
     $(".sidebar-menu a[href='/user-actions']").parents('li').addClass("active");
        getActionsList();
    }
});

//get total users till today
function getTotalUsers() {
    $.ajax({
        url: "/api/Chart/TotalUsers",
        async: true,
        success: function (data) {
            //console.log(data);
            var lineChartCanvas = $('#lineChart1').get(0).getContext('2d');
            var lineChartOptions = areaChartOptions;
            $("#TotalUsersCount").html(parseInt(data.yAxisAndroid) + parseInt(data.yAxisIphone));
            var areaChartData = {
                labels: ['', data.xAxis, ''],
                datasets: [
                  {
                      label: 'Android',
                      borderColor: 'rgba(96, 154, 235, 1)',
                      backgroundColor: 'rgba(96, 154, 235, 1)',
                      fill: false,
                      data: [null, data.yAxisAndroid, null]
                  },
                  {
                      label: 'ios',
                      borderColor: 'rgba(0,158,15,1)',
                      backgroundColor: 'rgba(0,158,15,1)',
                      fill: false,
                      data: [null, data.yAxisIphone, null]
                  }
                ]
            }
            Chart.Line(lineChartCanvas, {
                data: areaChartData,
                options: lineChartOptions
            });
        }
    });
}

//get new users till today
function getNewUsers(type) {
    $.ajax({
        url: "/api/Chart/NewUsers?type=" + type,
        async: true,
        success: function (data) {
            //console.log(data);
            var lineChartCanvas = $('#lineChart2').get(0).getContext('2d');
            var lineChartOptions = areaChartOptions;

            var areaChartData = {};
            if (type == 0) {
                $("#NewUsersCount").html(parseInt(data[0].yAxisAndroid) + parseInt(data[0].yAxisIphone));
                areaChartData = {
                    labels: ['', data[0].xAxis, ''],
                    datasets: [
                      {
                          label: 'Android',
                          borderColor: 'rgba(96, 154, 235, 1)',
                          backgroundColor: 'rgba(96, 154, 235, 1)',
                          fill: false,
                          data: [null, data[0].yAxisAndroid, null]
                      },
                      {
                          label: 'ios',
                          borderColor: 'rgba(0,158,15,1)',
                          backgroundColor: 'rgba(0,158,15,1)',
                          fill: false,
                          data: [null, data[0].yAxisIphone, null]
                      }
                    ]
                }
            }
            else {
                var xAxisArr = [];
                var yAxisAndroidArr = [];
                var yAxisIosArr = [];
                var total = 0;
                for (var i = 0; i < data.length; i++) {
                    xAxisArr.push(data[i].xAxis);
                    yAxisAndroidArr.push(data[i].yAxisAndroid);
                    yAxisIosArr.push(data[i].yAxisIphone);
                    total += parseInt(data[i].yAxisAndroid) + parseInt(data[i].yAxisIphone);
                }
                $("#NewUsersCount").html(total);
                areaChartData = {
                    labels: xAxisArr,
                    datasets: [
                      {
                          label: 'Android',
                          borderColor: 'rgba(96, 154, 235, 1)',
                          backgroundColor: 'rgba(96, 154, 235, 1)',
                          fill: false,
                          data: yAxisAndroidArr
                      },
                      {
                          label: 'ios',
                          borderColor: 'rgba(0,158,15,1)',
                          backgroundColor: 'rgba(0,158,15,1)',
                          fill: false,
                          data: yAxisIosArr
                      }
                    ]
                }
            }
            Chart.Line(lineChartCanvas, {
                data: areaChartData,
                options: lineChartOptions
            });
        }
    });
}

function LoadChartData(type, obj) {
    if (!$(obj).hasClass("active")) {
        $(".btn-chart-data button").removeClass("active");
        $(obj).addClass("active");
        getNewUsers(type);
        getActiveUsers(type);
        getTerminatedUsers(type);
        getRevenueChart(type);
        getInappPurchases(type);
    }
    return false;
}

//get Active users till today
function getActiveUsers(type) {
    $.ajax({
        url: "/api/Chart/ActiveUsers?type=" + type,
        async: true,
        success: function (data) {
            //console.log(data);
            var lineChartCanvas = $('#lineChart3').get(0).getContext('2d');
            var lineChartOptions = areaChartOptions;

            var areaChartData = {};
            if (type == 0) {
                $("#ActiveUsersCount").html(parseInt(data[0].yAxisAndroid) + parseInt(data[0].yAxisIphone));
                areaChartData = {
                    labels: ['', data[0].xAxis, ''],
                    datasets: [
                      {
                          label: 'Android',
                          borderColor: 'rgba(96, 154, 235, 1)',
                          backgroundColor: 'rgba(96, 154, 235, 1)',
                          fill: false,
                          data: [null, data[0].yAxisAndroid, null]
                      },
                      {
                          label: 'ios',
                          borderColor: 'rgba(0,158,15,1)',
                          backgroundColor: 'rgba(0,158,15,1)',
                          fill: false,
                          data: [null, data[0].yAxisIphone, null]
                      }
                    ]
                }
            }
            else {
                var xAxisArr = [];
                var yAxisAndroidArr = [];
                var yAxisIosArr = [];
                var total = 0;
                for (var i = 0; i < data.length; i++) {
                    xAxisArr.push(data[i].xAxis);
                    yAxisAndroidArr.push(data[i].yAxisAndroid);
                    yAxisIosArr.push(data[i].yAxisIphone);
                    total += parseInt(data[i].yAxisAndroid) + parseInt(data[i].yAxisIphone);
                }
                $("#ActiveUsersCount").html(total);
                areaChartData = {
                    labels: xAxisArr,
                    datasets: [
                      {
                          label: 'Android',
                          borderColor: 'rgba(96, 154, 235, 1)',
                          backgroundColor: 'rgba(96, 154, 235, 1)',
                          fill: false,
                          data: yAxisAndroidArr
                      },
                      {
                          label: 'ios',
                          borderColor: 'rgba(0,158,15,1)',
                          backgroundColor: 'rgba(0,158,15,1)',
                          fill: false,
                          data: yAxisIosArr
                      }
                    ]
                }
            }
            Chart.Line(lineChartCanvas, {
                data: areaChartData,
                options: lineChartOptions
            });
        }
    });
}

//get terminated users till today
function getTerminatedUsers(type) {
    $.ajax({
        url: "/api/Chart/TerminatedUsers?type=" + type,
        async: true,
        success: function (data) {
            //console.log(data);
            var lineChartCanvas = $('#lineChart4').get(0).getContext('2d');
            var lineChartOptions = areaChartOptions;

            var areaChartData = {};
            if (type == 0) {
                $("#TerminatedUsersCount").html(parseInt(data[0].yAxisAndroid) + parseInt(data[0].yAxisIphone));
                areaChartData = {
                    labels: ['', data[0].xAxis, ''],
                    datasets: [
                      {
                          label: 'Android',
                          borderColor: 'rgba(96, 154, 235, 1)',
                          backgroundColor: 'rgba(96, 154, 235, 1)',
                          fill: false,
                          data: [null, data[0].yAxisAndroid, null]
                      },
                      {
                          label: 'ios',
                          borderColor: 'rgba(0,158,15,1)',
                          backgroundColor: 'rgba(0,158,15,1)',
                          fill: false,
                          data: [null, data[0].yAxisIphone, null]
                      }
                    ]
                }
            }
            else {
                var xAxisArr = [];
                var yAxisAndroidArr = [];
                var yAxisIosArr = [];
                var total = 0;
                for (var i = 0; i < data.length; i++) {
                    xAxisArr.push(data[i].xAxis);
                    yAxisAndroidArr.push(data[i].yAxisAndroid);
                    yAxisIosArr.push(data[i].yAxisIphone);
                    total += parseInt(data[i].yAxisAndroid) + parseInt(data[i].yAxisIphone);
                }
                $("#TerminatedUsersCount").html(total);
                areaChartData = {
                    labels: xAxisArr,
                    datasets: [
                      {
                          label: 'Android',
                          borderColor: 'rgba(96, 154, 235, 1)',
                          backgroundColor: 'rgba(96, 154, 235, 1)',
                          fill: false,
                          data: yAxisAndroidArr
                      },
                      {
                          label: 'ios',
                          borderColor: 'rgba(0,158,15,1)',
                          backgroundColor: 'rgba(0,158,15,1)',
                          fill: false,
                          data: yAxisIosArr
                      }
                    ]
                }
            }
            Chart.Line(lineChartCanvas, {
                data: areaChartData,
                options: lineChartOptions
            });
        }
    });
}

//get revenue chart
function getRevenueChart(type) {
    $.ajax({
        url: "/api/Chart/revenuechart?type=" + type,
        async: true,
        success: function (data) {
            //console.log(data);
            var lineChartCanvas = $('#lineChart5').get(0).getContext('2d');
            var lineChartOptions = areaChartOptions;

            var areaChartData = {};
            if (type == 0) {
                $("#TotalRevenueCount").html(parseFloat(data[0].yAxisAndroid) + parseFloat(data[0].yAxisIphone));
                areaChartData = {
                    labels: ['', data[0].xAxis, ''],
                    datasets: [
                        {
                            label: 'Android',
                            borderColor: 'rgba(96, 154, 235, 1)',
                            backgroundColor: 'rgba(96, 154, 235, 1)',
                            fill: false,
                            data: [null, data[0].yAxisAndroid, null]
                        },
                        {
                            label: 'ios',
                            borderColor: 'rgba(0,158,15,1)',
                            backgroundColor: 'rgba(0,158,15,1)',
                            fill: false,
                            data: [null, data[0].yAxisIphone, null]
                        }
                    ]
                }
            }
            else {
                var xAxisArr = [];
                var yAxisAndroidArr = [];
                var yAxisIosArr = [];
                var total = 0;
                for (var i = 0; i < data.length; i++) {
                    xAxisArr.push(data[i].xAxis);
                    yAxisAndroidArr.push(data[i].yAxisAndroid);
                    yAxisIosArr.push(data[i].yAxisIphone);
                    total += parseFloat(data[i].yAxisAndroid) + parseFloat(data[i].yAxisIphone);
                }
                $("#TotalRevenueCount").html(total.toFixed(2));
                areaChartData = {
                    labels: xAxisArr,
                    datasets: [
                        {
                            label: 'Android',
                            borderColor: 'rgba(96, 154, 235, 1)',
                            backgroundColor: 'rgba(96, 154, 235, 1)',
                            fill: false,
                            data: yAxisAndroidArr
                        },
                        {
                            label: 'ios',
                            borderColor: 'rgba(0,158,15,1)',
                            backgroundColor: 'rgba(0,158,15,1)',
                            fill: false,
                            data: yAxisIosArr
                        }
                    ]
                }
            }
            Chart.Line(lineChartCanvas, {
                data: areaChartData,
                options: lineChartOptions
            });
        }
    });
}

//get Inapp purchases
function getInappPurchases(type) {
    $.ajax({
        url: "/api/Chart/inappchart?type=" + type,
        async: true,
        success: function (data) {
            //console.log(data);
            var lineChartCanvas = $('#lineChart6').get(0).getContext('2d');
            var lineChartOptions = areaChartOptions;

            var areaChartData = {};
            if (type == 0) {
                $("#TotalInAppCount").html(parseInt(data[0].yAxisAndroid) + parseInt(data[0].yAxisIphone));
                areaChartData = {
                    labels: ['', data[0].xAxis, ''],
                    datasets: [
                        {
                            label: 'Android',
                            borderColor: 'rgba(96, 154, 235, 1)',
                            backgroundColor: 'rgba(96, 154, 235, 1)',
                            fill: false,
                            data: [null, data[0].yAxisAndroid, null]
                        },
                        {
                            label: 'ios',
                            borderColor: 'rgba(0,158,15,1)',
                            backgroundColor: 'rgba(0,158,15,1)',
                            fill: false,
                            data: [null, data[0].yAxisIphone, null]
                        }
                    ]
                }
            }
            else {
                var xAxisArr = [];
                var yAxisAndroidArr = [];
                var yAxisIosArr = [];
                var total = 0;
                for (var i = 0; i < data.length; i++) {
                    xAxisArr.push(data[i].xAxis);
                    yAxisAndroidArr.push(data[i].yAxisAndroid);
                    yAxisIosArr.push(data[i].yAxisIphone);
                    total += parseInt(data[i].yAxisAndroid) + parseInt(data[i].yAxisIphone);
                }
                $("#TotalInAppCount").html(total);
                areaChartData = {
                    labels: xAxisArr,
                    datasets: [
                        {
                            label: 'Android',
                            borderColor: 'rgba(96, 154, 235, 1)',
                            backgroundColor: 'rgba(96, 154, 235, 1)',
                            fill: false,
                            data: yAxisAndroidArr
                        },
                        {
                            label: 'ios',
                            borderColor: 'rgba(0,158,15,1)',
                            backgroundColor: 'rgba(0,158,15,1)',
                            fill: false,
                            data: yAxisIosArr
                        }
                    ]
                }
            }
            Chart.Line(lineChartCanvas, {
                data: areaChartData,
                options: lineChartOptions
            });
        }
    });
}

//get add on features
function getAddonFeatures() {
    $.ajax({
        url: "/api/Chart/addonfeature",
        async: true,
        success: function (data) {
            //console.log(data);
            var html = '<thead>';
            html += '<tr>';
            html += '<th>Plan</th>';
            html += '<th>Current</th>';
            html += '<th>Active</th>';
            html += '<th>Expired</th>';
            html += '</tr>';
            html += '</thead>';
            html += '<tbody>';
            if (data.length > 0) {
                for (var i = 0; i < data.length; i++) {
                    if (i!=0 && data[i - 1].PackageFullName.indexOf("Parent") >= 0 && data[i].PackageFullName.indexOf("Child") >= 0) {
                        html += '<tr class="border-top">';
                    }
                    else {
                        html += '<tr>';
                    }
                    var pname = '';
                    pname += data[i].PackageFullName;
                    html += '<td>' + pname + '</td>';
                    html += '<td>' + data[i].CurrentTotal + '</td>';
                    html += '<td>' + data[i].ActiveTotal + '</td>';
                    html += '<td>' + data[i].ExpiredTotal + '</td>';
                    html += '</tr>';
                }
            }

            html += '</tbody>';

            $("#tblAddOnFeature").html(html);
        }
    });
}

//get users list
function getUsersList() {
    var html = '<thead>';
    html += '<tr>';
    html += '<th></th>';
    html += '<th>First Name</th>';
    html += '<th>Last Name</th>';
    html += '<th>Country</th>';
    html += '<th>Contact Number</th>';
    html += '<th>Email Address</th>';
    html += '<th>Facebook ID</th>';
    html += '<th>OS Type</th>';
    html += '<th>Account Status</th>';
    html += '<th>User Type</th>';
    html += '<th>Last Active Date</th>';
    html += '<th>Registration Date</th>';
    html += '<th>Add-on Purchases</th>';
    html += '<th>Action</th>';
    html += '</tr>';
    html += '</thead>';
    $('#example1').html(html);
    if ($.fn.dataTable.isDataTable('#example1')) {
        table.destroy();
    }
    table = $('#example1').DataTable({
        "proccessing": true,
        "serverSide": true,
        scrollY: "400px",
        scrollX: true,
        "searching": true,
        "paging": true,
        "info": false,
        fixedHeader: {
            header: true
        },
        fixedColumns: {
            leftColumns: 4
        },
        columnDefs: [{
            orderable: false,
            className: 'select-checkbox',
            targets: 0
        }],
        select: {
            style: 'os',
            selector: 'td:first-child'
        },
        order: [[1, 'asc']],
        "ajax": {
            url: "/Report/Users",
            type: 'POST',
            data:function ( d ) {
                d.start_time = $('#reportDate').val() != '' ? $('#reportDate').val().split(" - ")[0] : null;
                d.end_time = $('#reportDate').val() != '' ? $('#reportDate').val().split(" - ")[1] : null;
                d.sort_by = $('#selectFilter').select2("val");
            }
        },
        "language": {
            "search": "",
            "searchPlaceholder": "Search..."
        },
        "columns": [
             {
                 "data": function (row, type, set) {
                     return "";
                 }
             },
             { "data": "FirstName" },
             { "data": "LastName" },
             { "data": "Country" },
             { "data": "Phone" },
             { "data": "Email" },
             { "data": "FacebookID" },
             { "data": "OS" },
             { "data": "AccountStatus" },
             { "data": "UserType" },
             { "data": "LastActiveDate" },
             { "data": "RegistrationDate" },
             { "data": "AddonPurchases" },
             {
                 "data": function (row, type, set) {
                     var html = '';
                     html += '<a href="javascript:void(0);" class="btn btn-primary btn-sm" onclick="MoreInfo(' + row.id + ');">';
                     html += '<span class="fa fa-info"></span> More Information';
                     html += '</a>&nbsp;';
                     html += '<a href="javascript:void(0);" class="btn btn-primary btn-sm" onclick="ResetPasswordPop(' + row.id + ');">';
                     html += '<span class="fa fa-wrench"></span> Action';
                     html += '</a>';
                     return html;
                 }
             }
        ]
    });
    table.on('xhr', function () {
        var json = table.ajax.json();
        $("#spnTotalUsers").html(json.recordsTotal);
        $("#spnIosUsers").html(json.ios);
        $("#spnAndroidUsers").html(json.android);
        var cArr = json.country.split(",");
        $("#divCountryUsers").html("");
        for (var i = 0; i < cArr.length; i++) {
            if (cArr[i].trim().indexOf(":") == 0) {
                $("#divCountryUsers").append("Other " + cArr[i].trim() + "<br/>");
            }
            else {
                $("#divCountryUsers").append(cArr[i].trim() + "<br/>");
            }
        }
    });
}

function onReportChange() {
    if ($("#selectReport").val() == "All Users") {
        getUsersList();
    }
    else if ($("#selectReport").val() == "Users by Country") {
        getUsersByCountryList();
    }
    else if ($("#selectReport").val() == "Active Users") {
        getActiveUsersList();
    }
    else if ($("#selectReport").val() == "In-app Purchases") {
        getInAppUsersList();
    }
    else if ($("#selectReport").val() == "Gross Revenue") {
        getGrossAmountUsersList();
    }
    else if ($("#selectReport").val() == "Total Revenue") {
        getTotalAmountUsersList();
    }
    else if ($("#selectReport").val() == "Promotions") {
        getPromoCodeUsersList();
    }
    else if ($("#selectReport").val() == "Perks") {
        getPerksUsersList();
    }
    else if ($("#selectReport").val() == "Perks Redemption") {
        getRedeemUsersList();
    }
}

//get UsersByCountry list
function getUsersByCountryList() {
    var html = '<thead>';
    html += '<tr>';
    html += '<th></th>';
    html += '<th>First Name</th>';
    html += '<th>Last Name</th>';
    html += '<th>Country</th>';
    html += '<th>Contact Number</th>';
    html += '<th>Email Address</th>';
    html += '<th>Facebook ID</th>';
    html += '<th>OS Type</th>';
    html += '<th>Account Status</th>';
    html += '<th>User Type</th>';
    html += '<th>Last Active Date</th>';
    html += '<th>Registration Date</th>';
    html += '<th>Add-on Purchases</th>';
    html += '<th>Action</th>';
    html += '</tr>';
    html += '</thead>';
    $('#example1').html(html);
    if ($.fn.dataTable.isDataTable('#example1')) {
        table.destroy();
    }
    table = $('#example1').DataTable({
        "proccessing": true,
        "serverSide": true,
        scrollY: "400px",
        scrollX: true,
        "searching": true,
        "paging": true,
        "info": false,
        fixedHeader: {
            header: true
        },
        fixedColumns: {
            leftColumns: 4
        },
        columnDefs: [{
            orderable: false,
            className: 'select-checkbox',
            targets: 0
        }],
        select: {
            style: 'os',
            selector: 'td:first-child'
        },
        order: [[1, 'asc']],
        "ajax": {
            url: "/Report/UsersByCountry",
            type: 'POST',
            data: function (d) {
                d.start_time = $('#reportDate').val() != '' ? $('#reportDate').val().split(" - ")[0] : null;
                d.end_time = $('#reportDate').val() != '' ? $('#reportDate').val().split(" - ")[1] : null;
                d.sort_by = $('#selectFilter').select2("val");
            }
        },
        "language": {
            "search": "",
            "searchPlaceholder": "Search..."
        },
        "columns": [
             {
                 "data": function (row, type, set) {
                     return "";
                 }
             },
             { "data": "FirstName" },
             { "data": "LastName" },
             { "data": "Country" },
             { "data": "Phone" },
             { "data": "Email" },
             { "data": "FacebookID" },
             { "data": "OS" },
             { "data": "AccountStatus" },
             { "data": "UserType" },
             { "data": "LastActiveDate" },
             { "data": "RegistrationDate" },
             { "data": "AddonPurchases" },
             {
                 "data": function (row, type, set) {
                     var html = '';
                     html += '<a href="javascript:void(0);" class="btn btn-primary btn-sm" onclick="MoreInfo(' + row.id + ');">';
                     html += '<span class="fa fa-info"></span> More Information';
                     html += '</a>&nbsp;';
                     html += '<a href="javascript:void(0);" class="btn btn-primary btn-sm" onclick="ResetPasswordPop(' + row.id + ');">';
                     html += '<span class="fa fa-wrench"></span> Action';
                     html += '</a>';
                     return html;
                 }
             }
        ]
    });
    table.on('xhr', function () {
        var json = table.ajax.json();
        $("#spnTotalUsers").html(json.recordsTotal);
        $("#spnIosUsers").html(json.ios);
        $("#spnAndroidUsers").html(json.android);
        var cArr = json.country.split(",");
        $("#divCountryUsers").html("");
        for (var i = 0; i < cArr.length; i++) {
            if (cArr[i].trim().indexOf(":") == 0) {
                $("#divCountryUsers").append("Other " + cArr[i].trim() + "<br/>");
            }
            else {
                $("#divCountryUsers").append(cArr[i].trim() + "<br/>");
            }
        }
    });
}

//get Active Users list
function getActiveUsersList() {
    var html = '<thead>';
    html += '<tr>';
    html += '<th></th>';
    html += '<th>First Name</th>';
    html += '<th>Last Name</th>';
    html += '<th>Country</th>';
    html += '<th>Contact Number</th>';
    html += '<th>Email Address</th>';
    html += '<th>Facebook ID</th>';
    html += '<th>OS Type</th>';
    html += '<th>Account Status</th>';
    html += '<th>User Type</th>';
    html += '<th>Last Active Date</th>';
    html += '<th>Registration Date</th>';
    html += '<th>Add-on Purchases</th>';
    html += '<th>Action</th>';
    html += '</tr>';
    html += '</thead>';
    $('#example1').html(html);
    if ($.fn.dataTable.isDataTable('#example1')) {
        table.destroy();
    }
    table = $('#example1').DataTable({
        "proccessing": true,
        "serverSide": true,
        scrollY: "400px",
        scrollX: true,
        "searching": true,
        "paging": true,
        "info": false,
        fixedHeader: {
            header: true
        },
        fixedColumns: {
            leftColumns: 4
        },
        columnDefs: [{
            orderable: false,
            className: 'select-checkbox',
            targets: 0
        }],
        select: {
            style: 'os',
            selector: 'td:first-child'
        },
        order: [[1, 'asc']],
        "ajax": {
            url: "/Report/ActiveUsers",
            type: 'POST',
            data: function (d) {
                d.start_time = $('#reportDate').val() != '' ? $('#reportDate').val().split(" - ")[0] : null;
                d.end_time = $('#reportDate').val() != '' ? $('#reportDate').val().split(" - ")[1] : null;
                d.sort_by = $('#selectFilter').select2("val");
            }
        },
        "language": {
            "search": "",
            "searchPlaceholder": "Search..."
        },
        "columns": [
             {
                 "data": function (row, type, set) {
                     return "";
                 }
             },
             { "data": "FirstName" },
             { "data": "LastName" },
             { "data": "Country" },
             { "data": "Phone" },
             { "data": "Email" },
             { "data": "FacebookID" },
             { "data": "OS" },
             { "data": "AccountStatus" },
             { "data": "UserType" },
             { "data": "LastActiveDate" },
             { "data": "RegistrationDate" },
             { "data": "AddonPurchases" },
             {
                 "data": function (row, type, set) {
                     var html = '';
                     html += '<a href="javascript:void(0);" class="btn btn-primary btn-sm" onclick="MoreInfo(' + row.id + ');">';
                     html += '<span class="fa fa-info"></span> More Information';
                     html += '</a>&nbsp;';
                     html += '<a href="javascript:void(0);" class="btn btn-primary btn-sm" onclick="ResetPasswordPop(' + row.id + ');">';
                     html += '<span class="fa fa-wrench"></span> Action';
                     html += '</a>';
                     return html;
                 }
             }
        ]
    });
    table.on('xhr', function () {
        var json = table.ajax.json();
        $("#spnTotalUsers").html(json.recordsTotal);
        $("#spnIosUsers").html(json.ios);
        $("#spnAndroidUsers").html(json.android);
        var cArr = json.country.split(",");
        $("#divCountryUsers").html("");
        for (var i = 0; i < cArr.length; i++) {
            if (cArr[i].trim().indexOf(":") == 0) {
                $("#divCountryUsers").append("Other " + cArr[i].trim() + "<br/>");
            }
            else {
                $("#divCountryUsers").append(cArr[i].trim() + "<br/>");
            }
        }
    });
}

//get InApp Users list
function getInAppUsersList() {
    var html = '<thead>';
    html += '<tr>';
    html += '<th></th>';
    html += '<th>First Name</th>';
    html += '<th>Last Name</th>';
    html += '<th>Country</th>';
    html += '<th>Contact Number</th>';
    html += '<th>Email Address</th>';
    html += '<th>Facebook ID</th>';
    html += '<th>OS Type</th>';
    html += '<th>Account Status</th>';
    html += '<th>User Type</th>';
    html += '<th>Last Active Date</th>';
    html += '<th>Registration Date</th>';
    html += '<th>Add-on Purchases</th>';
    html += '<th>Action</th>';
    html += '</tr>';
    html += '</thead>';
    $('#example1').html(html);
    if ($.fn.dataTable.isDataTable('#example1')) {
        table.destroy();
    }
    table = $('#example1').DataTable({
        "proccessing": true,
        "serverSide": true,
        scrollY: "400px",
        scrollX: true,
        "searching": true,
        "paging": true,
        "info": false,
        fixedHeader: {
            header: true
        },
        fixedColumns: {
            leftColumns: 4
        },
        columnDefs: [{
            orderable: false,
            className: 'select-checkbox',
            targets: 0
        }],
        select: {
            style: 'os',
            selector: 'td:first-child'
        },
        order: [[1, 'asc']],
        "ajax": {
            url: "/Report/InAppUsers",
            type: 'POST',
            data: function (d) {
                d.start_time = $('#reportDate').val() != '' ? $('#reportDate').val().split(" - ")[0] : null;
                d.end_time = $('#reportDate').val() != '' ? $('#reportDate').val().split(" - ")[1] : null;
                d.sort_by = $('#selectFilter').select2("val");
            }
        },
        "language": {
            "search": "",
            "searchPlaceholder": "Search..."
        },
        "columns": [
             {
                 "data": function (row, type, set) {
                     return "";
                 }
             },
             { "data": "FirstName" },
             { "data": "LastName" },
             { "data": "Country" },
             { "data": "Phone" },
             { "data": "Email" },
             { "data": "FacebookID" },
             { "data": "OS" },
             { "data": "AccountStatus" },
             { "data": "UserType" },
             { "data": "LastActiveDate" },
             { "data": "RegistrationDate" },
             { "data": "AddonPurchases" },
             {
                 "data": function (row, type, set) {
                     var html = '';
                     html += '<a href="javascript:void(0);" class="btn btn-primary btn-sm" onclick="MoreInfo(' + row.id + ');">';
                     html += '<span class="fa fa-info"></span> More Information';
                     html += '</a>&nbsp;';
                     html += '<a href="javascript:void(0);" class="btn btn-primary btn-sm" onclick="ResetPasswordPop(' + row.id + ');">';
                     html += '<span class="fa fa-wrench"></span> Action';
                     html += '</a>';
                     return html;
                 }
             }
        ]
    });
    table.on('xhr', function () {
        var json = table.ajax.json();
        $("#spnTotalUsers").html(json.recordsTotal);
        $("#spnIosUsers").html(json.ios);
        $("#spnAndroidUsers").html(json.android);
        var cArr = json.country.split(",");
        $("#divCountryUsers").html("");
        for (var i = 0; i < cArr.length; i++) {
            if (cArr[i].trim().indexOf(":") == 0) {
                $("#divCountryUsers").append("Other " + cArr[i].trim() + "<br/>");
            }
            else {
                $("#divCountryUsers").append(cArr[i].trim() + "<br/>");
            }
        }
    });
}

//get GrossAmount Users list
function getGrossAmountUsersList() {
    var html = '<thead>';
    html += '<tr>';
    html += '<th></th>';
    html += '<th>First Name</th>';
    html += '<th>Last Name</th>';
    html += '<th>Country</th>';
    html += '<th>Contact Number</th>';
    html += '<th>Email Address</th>';
    html += '<th>Facebook ID</th>';
    html += '<th>OS Type</th>';
    html += '<th>Account Status</th>';
    html += '<th>User Type</th>';
    html += '<th>Last Active Date</th>';
    html += '<th>Registration Date</th>';
    html += '<th>Add-on Purchases</th>';
    html += '<th>Action</th>';
    html += '</tr>';
    html += '</thead>';
    $('#example1').html(html);
    if ($.fn.dataTable.isDataTable('#example1')) {
        table.destroy();
    }
    table = $('#example1').DataTable({
        "proccessing": true,
        "serverSide": true,
        scrollY: "400px",
        scrollX: true,
        "searching": true,
        "paging": true,
        "info": false,
        fixedHeader: {
            header: true
        },
        fixedColumns: {
            leftColumns: 4
        },
        columnDefs: [{
            orderable: false,
            className: 'select-checkbox',
            targets: 0
        }],
        select: {
            style: 'os',
            selector: 'td:first-child'
        },
        order: [[1, 'asc']],
        "ajax": {
            url: "/Report/GrossUsers",
            type: 'POST',
            data: function (d) {
                d.start_time = $('#reportDate').val() != '' ? $('#reportDate').val().split(" - ")[0] : null;
                d.end_time = $('#reportDate').val() != '' ? $('#reportDate').val().split(" - ")[1] : null;
                d.sort_by = $('#selectFilter').select2("val");
            }
        },
        "language": {
            "search": "",
            "searchPlaceholder": "Search..."
        },
        "columns": [
             {
                 "data": function (row, type, set) {
                     return "";
                 }
             },
             { "data": "FirstName" },
             { "data": "LastName" },
             { "data": "Country" },
             { "data": "Phone" },
             { "data": "Email" },
             { "data": "FacebookID" },
             { "data": "OS" },
             { "data": "AccountStatus" },
             { "data": "UserType" },
             { "data": "LastActiveDate" },
             { "data": "RegistrationDate" },
             { "data": "AddonPurchases" },
             {
                 "data": function (row, type, set) {
                     var html = '';
                     html += '<a href="javascript:void(0);" class="btn btn-primary btn-sm" onclick="MoreInfo(' + row.id + ');">';
                     html += '<span class="fa fa-info"></span> More Information';
                     html += '</a>&nbsp;';
                     html += '<a href="javascript:void(0);" class="btn btn-primary btn-sm" onclick="ResetPasswordPop(' + row.id + ');">';
                     html += '<span class="fa fa-wrench"></span> Action';
                     html += '</a>';
                     return html;
                 }
             }
        ]
    });
    table.on('xhr', function () {
        var json = table.ajax.json();
        $("#spnTotalUsers").html(json.recordsTotal);
        $("#spnIosUsers").html(json.ios);
        $("#spnAndroidUsers").html(json.android);
        var cArr = json.country.split(",");
        $("#divCountryUsers").html("");
        for (var i = 0; i < cArr.length; i++) {
            if (cArr[i].trim().indexOf(":") == 0) {
                $("#divCountryUsers").append("Other " + cArr[i].trim() + "<br/>");
            }
            else {
                $("#divCountryUsers").append(cArr[i].trim() + "<br/>");
            }
        }
    });
}

//get TotalAmount Users list
function getTotalAmountUsersList() {
    var html = '<thead>';
    html += '<tr>';
    html += '<th></th>';
    html += '<th>First Name</th>';
    html += '<th>Last Name</th>';
    html += '<th>Country</th>';
    html += '<th>Contact Number</th>';
    html += '<th>Email Address</th>';
    html += '<th>Facebook ID</th>';
    html += '<th>OS Type</th>';
    html += '<th>Account Status</th>';
    html += '<th>User Type</th>';
    html += '<th>Last Active Date</th>';
    html += '<th>Registration Date</th>';
    html += '<th>Add-on Purchases</th>';
    html += '<th>Action</th>';
    html += '</tr>';
    html += '</thead>';
    $('#example1').html(html);
    if ($.fn.dataTable.isDataTable('#example1')) {
        table.destroy();
    }
    table = $('#example1').DataTable({
        "proccessing": true,
        "serverSide": true,
        scrollY: "400px",
        scrollX: true,
        "searching": true,
        "paging": true,
        "info": false,
        fixedHeader: {
            header: true
        },
        fixedColumns: {
            leftColumns: 4
        },
        columnDefs: [{
            orderable: false,
            className: 'select-checkbox',
            targets: 0
        }],
        select: {
            style: 'os',
            selector: 'td:first-child'
        },
        order: [[1, 'asc']],
        "ajax": {
            url: "/Report/TotalAmountUsers",
            type: 'POST',
            data: function (d) {
                d.start_time = $('#reportDate').val() != '' ? $('#reportDate').val().split(" - ")[0] : null;
                d.end_time = $('#reportDate').val() != '' ? $('#reportDate').val().split(" - ")[1] : null;
                d.sort_by = $('#selectFilter').select2("val");
            }
        },
        "language": {
            "search": "",
            "searchPlaceholder": "Search..."
        },
        "columns": [
             {
                 "data": function (row, type, set) {
                     return "";
                 }
             },
             { "data": "FirstName" },
             { "data": "LastName" },
             { "data": "Country" },
             { "data": "Phone" },
             { "data": "Email" },
             { "data": "FacebookID" },
             { "data": "OS" },
             { "data": "AccountStatus" },
             { "data": "UserType" },
             { "data": "LastActiveDate" },
             { "data": "RegistrationDate" },
             { "data": "AddonPurchases" },
             {
                 "data": function (row, type, set) {
                     var html = '';
                     html += '<a href="javascript:void(0);" class="btn btn-primary btn-sm" onclick="MoreInfo(' + row.id + ');">';
                     html += '<span class="fa fa-info"></span> More Information';
                     html += '</a>&nbsp;';
                     html += '<a href="javascript:void(0);" class="btn btn-primary btn-sm" onclick="ResetPasswordPop(' + row.id + ');">';
                     html += '<span class="fa fa-wrench"></span> Action';
                     html += '</a>';
                     return html;
                 }
             }
        ]
    });
    table.on('xhr', function () {
        var json = table.ajax.json();
        $("#spnTotalUsers").html(json.recordsTotal);
        $("#spnIosUsers").html(json.ios);
        $("#spnAndroidUsers").html(json.android);
        var cArr = json.country.split(",");
        $("#divCountryUsers").html("");
        for (var i = 0; i < cArr.length; i++) {
            if (cArr[i].trim().indexOf(":") == 0) {
                $("#divCountryUsers").append("Other " + cArr[i].trim() + "<br/>");
            }
            else {
                $("#divCountryUsers").append(cArr[i].trim() + "<br/>");
            }
        }
    });
}

//get PromoCode Users list
function getPromoCodeUsersList() {
    var html = '<thead>';
    html += '<tr>';
    html += '<th></th>';
    html += '<th>First Name</th>';
    html += '<th>Last Name</th>';
    html += '<th>Country</th>';
    html += '<th>Contact Number</th>';
    html += '<th>Email Address</th>';
    html += '<th>Facebook ID</th>';
    html += '<th>OS Type</th>';
    html += '<th>Account Status</th>';
    html += '<th>User Type</th>';
    html += '<th>Last Active Date</th>';
    html += '<th>Registration Date</th>';
    html += '<th>Add-on Purchases</th>';
    html += '<th>Action</th>';
    html += '</tr>';
    html += '</thead>';
    $('#example1').html(html);
    if ($.fn.dataTable.isDataTable('#example1')) {
        table.destroy();
    }
    table = $('#example1').DataTable({
        "proccessing": true,
        "serverSide": true,
        scrollY: "400px",
        scrollX: true,
        "searching": true,
        "paging": true,
        "info": false,
        fixedHeader: {
            header: true
        },
        fixedColumns: {
            leftColumns: 4
        },
        columnDefs: [{
            orderable: false,
            className: 'select-checkbox',
            targets: 0
        }],
        select: {
            style: 'os',
            selector: 'td:first-child'
        },
        order: [[1, 'asc']],
        "ajax": {
            url: "/Report/PromoCodeUsers",
            type: 'POST',
            data: function (d) {
                d.start_time = $('#reportDate').val() != '' ? $('#reportDate').val().split(" - ")[0] : null;
                d.end_time = $('#reportDate').val() != '' ? $('#reportDate').val().split(" - ")[1] : null;
                d.sort_by = $('#selectFilter').select2("val");
            }
        },
        "language": {
            "search": "",
            "searchPlaceholder": "Search..."
        },
        "columns": [
             {
                 "data": function (row, type, set) {
                     return "";
                 }
             },
             { "data": "FirstName" },
             { "data": "LastName" },
             { "data": "Country" },
             { "data": "Phone" },
             { "data": "Email" },
             { "data": "FacebookID" },
             { "data": "OS" },
             { "data": "AccountStatus" },
             { "data": "UserType" },
             { "data": "LastActiveDate" },
             { "data": "RegistrationDate" },
             { "data": "AddonPurchases" },
             {
                 "data": function (row, type, set) {
                     var html = '';
                     html += '<a href="javascript:void(0);" class="btn btn-primary btn-sm" onclick="MoreInfo(' + row.id + ');">';
                     html += '<span class="fa fa-info"></span> More Information';
                     html += '</a>&nbsp;';
                     html += '<a href="javascript:void(0);" class="btn btn-primary btn-sm" onclick="ResetPasswordPop(' + row.id + ');">';
                     html += '<span class="fa fa-wrench"></span> Action';
                     html += '</a>';
                     return html;
                 }
             }
        ]
    });
    table.on('xhr', function () {
        var json = table.ajax.json();
        $("#spnTotalUsers").html(json.recordsTotal);
        $("#spnIosUsers").html(json.ios);
        $("#spnAndroidUsers").html(json.android);
        var cArr = json.country.split(",");
        $("#divCountryUsers").html("");
        for (var i = 0; i < cArr.length; i++) {
            if (cArr[i].trim().indexOf(":") == 0) {
                $("#divCountryUsers").append("Other " + cArr[i].trim() + "<br/>");
            }
            else {
                $("#divCountryUsers").append(cArr[i].trim() + "<br/>");
            }
        }
    });
}

//get Perks Users list
function getPerksUsersList() {
    var html = '<thead>';
    html += '<tr>';
    html += '<th></th>';
    html += '<th>First Name</th>';
    html += '<th>Last Name</th>';
    html += '<th>Country</th>';
    html += '<th>Contact Number</th>';
    html += '<th>Email Address</th>';
    html += '<th>Facebook ID</th>';
    html += '<th>OS Type</th>';
    html += '<th>Account Status</th>';
    html += '<th>User Type</th>';
    html += '<th>Last Active Date</th>';
    html += '<th>Registration Date</th>';
    html += '<th>Add-on Purchases</th>';
    html += '<th>Action</th>';
    html += '</tr>';
    html += '</thead>';
    $('#example1').html(html);
    if ($.fn.dataTable.isDataTable('#example1')) {
        table.destroy();
    }
    table = $('#example1').DataTable({
        "proccessing": true,
        "serverSide": true,
        scrollY: "400px",
        scrollX: true,
        "searching": true,
        "paging": true,
        "info": false,
        fixedHeader: {
            header: true
        },
        fixedColumns: {
            leftColumns: 4
        },
        columnDefs: [{
            orderable: false,
            className: 'select-checkbox',
            targets: 0
        }],
        select: {
            style: 'os',
            selector: 'td:first-child'
        },
        order: [[1, 'asc']],
        "ajax": {
            url: "/Report/PerksUsers",
            type: 'POST',
            data: function (d) {
                d.start_time = $('#reportDate').val() != '' ? $('#reportDate').val().split(" - ")[0] : null;
                d.end_time = $('#reportDate').val() != '' ? $('#reportDate').val().split(" - ")[1] : null;
                d.sort_by = $('#selectFilter').select2("val");
            }
        },
        "language": {
            "search": "",
            "searchPlaceholder": "Search..."
        },
        "columns": [
             {
                 "data": function (row, type, set) {
                     return "";
                 }
             },
             { "data": "FirstName" },
             { "data": "LastName" },
             { "data": "Country" },
             { "data": "Phone" },
             { "data": "Email" },
             { "data": "FacebookID" },
             { "data": "OS" },
             { "data": "AccountStatus" },
             { "data": "UserType" },
             { "data": "LastActiveDate" },
             { "data": "RegistrationDate" },
             { "data": "AddonPurchases" },
             {
                 "data": function (row, type, set) {
                     var html = '';
                     html += '<a href="javascript:void(0);" class="btn btn-primary btn-sm" onclick="MoreInfo(' + row.id + ');">';
                     html += '<span class="fa fa-info"></span> More Information';
                     html += '</a>&nbsp;';
                     html += '<a href="javascript:void(0);" class="btn btn-primary btn-sm" onclick="ResetPasswordPop(' + row.id + ');">';
                     html += '<span class="fa fa-wrench"></span> Action';
                     html += '</a>';
                     return html;
                 }
             }
        ]
    });
    table.on('xhr', function () {
        var json = table.ajax.json();
        $("#spnTotalUsers").html(json.recordsTotal);
        $("#spnIosUsers").html(json.ios);
        $("#spnAndroidUsers").html(json.android);
        var cArr = json.country.split(",");
        $("#divCountryUsers").html("");
        for (var i = 0; i < cArr.length; i++) {
            if (cArr[i].trim().indexOf(":") == 0) {
                $("#divCountryUsers").append("Other " + cArr[i].trim() + "<br/>");
            }
            else {
                $("#divCountryUsers").append(cArr[i].trim() + "<br/>");
            }
        }
    });
}

//get Redeem Users list
function getRedeemUsersList() {
    var html = '<thead>';
    html += '<tr>';
    html += '<th></th>';
    html += '<th>First Name</th>';
    html += '<th>Last Name</th>';
    html += '<th>Country</th>';
    html += '<th>Contact Number</th>';
    html += '<th>Email Address</th>';
    html += '<th>Facebook ID</th>';
    html += '<th>OS Type</th>';
    html += '<th>Account Status</th>';
    html += '<th>User Type</th>';
    html += '<th>Last Active Date</th>';
    html += '<th>Registration Date</th>';
    html += '<th>Add-on Purchases</th>';
    html += '<th>Action</th>';
    html += '</tr>';
    html += '</thead>';
    $('#example1').html(html);
    if ($.fn.dataTable.isDataTable('#example1')) {
        table.destroy();
    }
    table = $('#example1').DataTable({
        "proccessing": true,
        "serverSide": true,
        scrollY: "400px",
        scrollX: true,
        "searching": true,
        "paging": true,
        "info": false,
        fixedHeader: {
            header: true
        },
        fixedColumns: {
            leftColumns: 4
        },
        columnDefs: [{
            orderable: false,
            className: 'select-checkbox',
            targets: 0
        }],
        select: {
            style: 'os',
            selector: 'td:first-child'
        },
        order: [[1, 'asc']],
        "ajax": {
            url: "/Report/RedeemUsers",
            type: 'POST',
            data: function (d) {
                d.start_time = $('#reportDate').val() != '' ? $('#reportDate').val().split(" - ")[0] : null;
                d.end_time = $('#reportDate').val() != '' ? $('#reportDate').val().split(" - ")[1] : null;
                d.sort_by = $('#selectFilter').select2("val");
            }
        },
        "language": {
            "search": "",
            "searchPlaceholder": "Search..."
        },
        "columns": [
             {
                 "data": function (row, type, set) {
                     return "";
                 }
             },
             { "data": "FirstName" },
             { "data": "LastName" },
             { "data": "Country" },
             { "data": "Phone" },
             { "data": "Email" },
             { "data": "FacebookID" },
             { "data": "OS" },
             { "data": "AccountStatus" },
             { "data": "UserType" },
             { "data": "LastActiveDate" },
             { "data": "RegistrationDate" },
             { "data": "AddonPurchases" },
             {
                 "data": function (row, type, set) {
                     var html = '';
                     html += '<a href="javascript:void(0);" class="btn btn-primary btn-sm" onclick="MoreInfo(' + row.id + ');">';
                     html += '<span class="fa fa-info"></span> More Information';
                     html += '</a>&nbsp;';
                     html += '<a href="javascript:void(0);" class="btn btn-primary btn-sm" onclick="ResetPasswordPop(' + row.id + ');">';
                     html += '<span class="fa fa-wrench"></span> Action';
                     html += '</a>';
                     return html;
                 }
             }
        ]
    });
    table.on('xhr', function () {
        var json = table.ajax.json();
        $("#spnTotalUsers").html(json.recordsTotal);
        $("#spnIosUsers").html(json.ios);
        $("#spnAndroidUsers").html(json.android);
        var cArr = json.country.split(",");
        $("#divCountryUsers").html("");
        for (var i = 0; i < cArr.length; i++) {
            if (cArr[i].trim().indexOf(":") == 0) {
                $("#divCountryUsers").append("Other " + cArr[i].trim() + "<br/>");
            }
            else {
                $("#divCountryUsers").append(cArr[i].trim() + "<br/>");
            }
        }
    });
}
//get promo codes list
function getPromoCodesList() {
    var html = '<thead>';
    html += '<tr>';
    html += '<th>Code ID</th>';
    html += '<th>Description</th>';
    html += '<th>Type</th>';
    html += '<th>Value</th>';
    html += '<th>Start Date</th>';
    html += '<th>End Date</th>';
    html += '<th>Quantity</th>';
    html += '<th>Rules</th>';
    html += '<th>Status</th>';
    html += '<th>Action</th>';
    html += '</tr>';
    html += '</thead>';
    $('#example1').html(html);
    if ($.fn.dataTable.isDataTable('#example1')) {
        table.destroy();
    }
    table = $('#example1').DataTable({
        "responsive": true,
        "proccessing": true,
        "serverSide": true,
        "searching": true,
        "paging": true,
        "info": false,
        fixedHeader: {
            header: true
        },
        order: [[0, 'asc']],
        columnDefs: [
       { orderable: false, targets: -1 }
        ],
        "ajax": {
            url: "/PromoCode/PromoCodes",
            type: 'POST'
        },
        "language": {
            "search": "",
            "searchPlaceholder": "Search..."
        },
        "columns": [
             { "data": "CodeID" },
             { "data": "Description" },
             { "data": "TypeValue" },
             { "data": "Value" },
             { "data": "StartDate" },
             { "data": "EndDate" },
             { "data": "Quantity" },
             { "data": "Rules" },
             { "data": "Status" },
             {
                 "data": function (row, type, set) {
                     var html = '';
                     html += '<a href="javascript:void(0);" class="btn btn-primary btn-sm" onclick="AddEditCode(' + row.id + ');">';
                     html += '<span class="fa fa-pencil"></span> Edit';
                     html += '</a>&nbsp;';
                     html += '<a href="javascript:void(0);" class="btn btn-primary btn-sm" onclick="DeleteCode(' + row.id + ');">';
                     html += '<span class="fa fa-trash"></span> Delete';
                     html += '</a>';
                     return html;
                 }
             }
        ]
    });
}

//get user redeems list
function getRedeemsList() {
    var html = '<thead>';
    html += '<tr>';
    html += '<th>User</th>';
    html += '<th>Redeem</th>';
    html += '<th>Date Redeem</th>';
    html += '<th>Status</th>';
    html += '<th>Action</th>';
    html += '</tr>';
    html += '</thead>';
    $('#example1').html(html);
    if ($.fn.dataTable.isDataTable('#example1')) {
        table.destroy();
    }
    table = $('#example1').DataTable({
        "responsive": true,
        "proccessing": true,
        "serverSide": true,
        "searching": true,
        "paging": true,
        "info": false,
        fixedHeader: {
            header: true
        },
        order: [[0, 'asc']],
        columnDefs: [
       { orderable: false, targets: -1 }
        ],
        "ajax": {
            url: "/Redeem/UserRedeems",
            type: 'POST'
        },
        "language": {
            "search": "",
            "searchPlaceholder": "Search..."
        },
        "columns": [
             { "data": "UserName" },
             { "data": "Redeem" },
             { "data": "DateRedeem" },
             {
                 "data": function (row, type, set) {
                     var html = '';
                     html += '<b>';
                     html += row.Status;
                     html += '</b>';
                     return html;
                 }
             },
             {
                 "data": function (row, type, set) {
                     var html = '';
                     if (row.ActionName != '') {
                         html += '<a href="javascript:void(0);" class="btn btn-primary btn-sm" onclick="ChangeRedeemStatus(' + row.id + ',\'' + row.ActionName + '\');">';
                         html += row.ActionName;
                         html += '</a>';
                     }
                     return html;
                 }
             }
        ]
    });
}

//get notifications list
function getNotificationsList() {
    var html = '<thead>';
    html += '<tr>';
    html += '<th>Name</th>';
    html += '<th>Send To</th>';
    html += '<th>Content</th>';
    html += '<th>Time Start</th>';
    html += '<th>Next Notification</th>';
    html += '<th>Type</th>';
    html += '<th>Status</th>';
    html += '<th>Action</th>';
    html += '</tr>';
    html += '</thead>';
    $('#example1').html(html);
    if ($.fn.dataTable.isDataTable('#example1')) {
        table.destroy();
    }
    table = $('#example1').DataTable({
        "responsive": true,
        "proccessing": true,
        "serverSide": true,
        "searching": true,
        "paging": true,
        "info": false,
        fixedHeader: {
            header: true
        },
        order: [[0, 'asc']],
        columnDefs: [
       { orderable: false, targets: -1 }
        ],
        "ajax": {
            url: "/notification/notifications",
            type: 'POST',
            "data": function (d) {
                d.CustomData = $("#ddlFilterNotification").val();
            }
        },
        "language": {
            "search": "",
            "searchPlaceholder": "Search..."
        },
        "columns": [
             { "data": "name" },
             {
                 "data": function (row, type, set) {
                     var h = row.data;
                     if (row.data != 'All user')
                        h += " <span class='fa fa-search' style='cursor:pointer;' title='more detail' onclick='ShowNotificationUsers(" + row.id + ")'></span>";
                     return h;
                 }
             },
             { "data": "content" },
             { "data": "OnDate" },
             { "data": "NextNotificationDate" },
             {
                 "data": function (row, type, set) {
                     return "<span style='text-transform:capitalize;'>" + row.type + "</span>";
                 }
             },
             {
                 "data": function (row, type, set) {
                     return "<span style='text-transform:capitalize;'>" + row.status + "</span>";
                 }
             },
             {
                 "data": function (row, type, set) {
                     var html = '';
                     html += '<a href="javascript:void(0);" class="btn btn-primary btn-sm" onclick="EditNotification(' + row.id + ');">';
                     html += '<span class="fa fa-pencil"></span> Edit';
                     html += '</a>&nbsp;';
                     if (row.status == "Draft") {
                         html += '<a href="javascript:void(0);" class="btn btn-primary btn-sm" onclick="PublishNotification(' + row.id + ');">';
                         html += '<span class="fa fa-upload"></span> Publish';
                         html += '</a>&nbsp;';
                     }
                     html += '<a href="javascript:void(0);" class="btn btn-primary btn-sm" onclick="DeleteNotification(' + row.id + ');">';
                     html += '<span class="fa fa-trash"></span> Delete';
                     html += '</a>';
                     return html;
                 }
             }
        ]
    });
}

//get languages list
function getLanguagesList() {
    var html = '<thead>';
    html += '<tr>';
    html += '<th>Key</th>';
    html += '<th>Label</th>';
    html += '<th>Value</th>';
    html += '<th>Action</th>';
    html += '</tr>';
    html += '</thead>';
    $('#example1').html(html);
    if ($.fn.dataTable.isDataTable('#example1')) {
        table.destroy();
    }
    table = $('#example1').DataTable({
        "responsive": true,
        "proccessing": true,
        "serverSide": true,
        "searching": true,
        "paging": true,
        "info": false,
        fixedHeader: {
            header: true
        },
        order: [[0, 'asc']],
        columnDefs: [
           { orderable: false, targets: -1 },
           {
               'targets': 1,
               'createdCell': function (td, cellData, rowData, row, col) {
                   $(td).attr('class', 'editable');
                   $(td).attr('data-type', 'key');
                   $(td).attr('data-id', rowData.key_id);
               }
           },
           {
               'targets': 2,
               'createdCell': function (td, cellData, rowData, row, col) {
                   $(td).attr('class', 'editable');
                   $(td).attr('data-type', 'translation');
                   $(td).attr('data-id', rowData.id);
               }
           }
        ],
        "ajax": {
            url: "/language/languagetranslations",
            type: 'POST',
            "data": function (d) {
                d.CustomData = $("#ddlFilterLanguage").val();
            }
        },
        "language": {
            "search": "",
            "searchPlaceholder": "Search..."
        },
        "columns": [
             { "data": "key" },
             { "data": "label" },
             { "data": "value" },
             {
                 "data": function (row, type, set) {
                     var html = '';
                     html += '<a href="javascript:void(0);" class="btn btn-primary btn-sm" onclick="DeleteKey(' + row.key_id + ');">';
                     html += '<span class="fa fa-trash"></span> Remove Key';
                     html += '</a>';
                     return html;
                 }
             }
        ],
        "drawCallback": function () {
            $('#example1').editableTableWidget();
        }
    });
}

//show custom message
function showMessage(type, message) {
    var url = "/Message/Show";

    $.ajax({
        type: "POST",
        url: url,
        data: '{MessageType: "' + type + '", MessageData:"' + message + '" }',
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (response) {
            $('#messageContainer').html(response);
        },
        failure: function (response) {
            //alert(response.responseText);
        },
        error: function (response) {
            //alert(response.responseText);
        }
    });
}

//refresh report
function refreshReport() {
    $("#selectFilter option").prop("selected", "selected");
    $("#selectFilter").trigger("change");
    var start = moment().subtract(29, 'days');
    var end = moment();
    $('#reportDate').data('daterangepicker').setStartDate(start);
    $('#reportDate').data('daterangepicker').setEndDate(end);
    onReportChange();
}

//get user redeems list
function getTasksList() {
    var html = '<thead>';
    html += '<tr>';
    html += '<th>Icon</th>';
    html += '<th>Name</th>';
    html += '<th>Number to finish</th>';
    html += '<th>Score</th>';
    html += '<th>Action</th>';
    html += '</tr>';
    html += '</thead>';
    $('#example1').html(html);
    if ($.fn.dataTable.isDataTable('#example1')) {
        table.destroy();
    }
    table = $('#example1').DataTable({
        "responsive": true,
        "proccessing": true,
        "serverSide": true,
        "searching": true,
        "paging": true,
        "info": false,
        fixedHeader: {
            header: true
        },
        order: [[1, 'asc']],
        columnDefs: [
       { orderable: false, targets: -1 },
       { orderable: false, targets: 0 }
        ],
        "ajax": {
            url: "/UserTasks/UserTasks",
            type: 'POST'
        },
        "language": {
            "search": "",
            "searchPlaceholder": "Search..."
        },
        "columns": [
             {
                 "data": function (row, type, set) {
                     var html = '';
                     html += '<img height="30" width="30" src="' + row.icon + '">';
                     return html;
                 }
             },
             { "data": "name" },
             { "data": "number_to_finish" },
             { "data": "score" },
             {
                 "data": function (row, type, set) {
                     var html = '';
                     html += '<a href="javascript:void(0);" class="btn btn-primary btn-sm" onclick="AddEditCode(' + row.id + ');">';
                     html += '<span class="fa fa-pencil"></span> Edit';
                     html += '</a>&nbsp;';
                     html += '<a href="javascript:void(0);" class="btn btn-primary btn-sm" onclick="DeleteCode(' + row.id + ');">';
                     html += '<span class="fa fa-trash"></span> Delete';
                     html += '</a>';
                     return html;
                 }
             }
        ]
    });
}

//get user redeems list
function getRedemptionList() {
    var html = '<thead>';
    html += '<tr>';
    html += '<th>Name</th>';
    html += '<th>Type</th>';
    html += '<th>Points</th>';
    html += '<th>Frequency</th>';
    html += '<th>Action</th>';
    html += '</tr>';
    html += '</thead>';
    $('#example1').html(html);
    if ($.fn.dataTable.isDataTable('#example1')) {
        table.destroy();
    }
    table = $('#example1').DataTable({
        "responsive": true,
        "proccessing": true,
        "serverSide": true,
        "searching": true,
        "paging": true,
        "info": false,
        fixedHeader: {
            header: true
        },
        order: [[0, 'asc']],
        columnDefs: [
       { orderable: false, targets: -1 }
        ],
        "ajax": {
            url: "/Redeem/RedemptionGifts",
            type: 'POST'
        },
        "language": {
            "search": "",
            "searchPlaceholder": "Search..."
        },
        "columns": [
             { "data": "name" },
             {
                 "data": function (row, type, set) {
                     if (row.type == 0) {
                         return "nonApp";
                     }
                     else {
                         return "inApp";
                     };
                 }
             },
             { "data": "points" },
             { "data": "frequency" },
             {
                 "data": function (row, type, set) {
                     var html = '';
                     html += '<a href="javascript:void(0);" class="btn btn-primary btn-sm" onclick="AddEditCode(' + row.id + ');">';
                     html += '<span class="fa fa-pencil"></span> Edit';
                     html += '</a>&nbsp;';
                     html += '<a href="javascript:void(0);" class="btn btn-primary btn-sm" onclick="DeleteCode(' + row.id + ');">';
                     html += '<span class="fa fa-trash"></span> Delete';
                     html += '</a>';
                     return html;
                 }
             }
        ]
    });
}

//get user plans list
function getPlansList() {
    var html = '<thead>';
    html += '<tr>';
    html += '<th>User Name</th>';
    html += '<th>User Email</th>';
    html += '<th>User Status</th>';
    html += '<th>Plan</th>';
    html += '<th>Promocode</th>';
    html += '<th>Start Date</th>';
    html += '</tr>';
    html += '</thead>';
    $('#example1').html(html);
    if ($.fn.dataTable.isDataTable('#example1')) {
        table.destroy();
    }
    table = $('#example1').DataTable({
        "responsive": true,
        "proccessing": true,
        "serverSide": true,
        "searching": true,
        "paging": true,
        "info": false,
        fixedHeader: {
            header: true
        },
        order: [[0, 'asc']],
        "ajax": {
            url: "/KpiReport/UserPlans",
            type: 'POST'
        },
        "language": {
            "search": "",
            "searchPlaceholder": "Search..."
        },
        "columns": [
             { "data": "Name" },
             { "data": "Email" },
             { "data": "Status" },
             { "data": "Plan" },
             { "data": "PromoCode" },
             { "data": "StartDate" }
        ]
    });
}

//get user redeems list
function getActionsList() {
    var html = '<thead>';
    html += '<tr>';
    html += '<th>User Name</th>';
    html += '<th>User Email</th>';
    html += '<th>Action</th>';
    html += '<th>Start Date</th>';
    html += '</tr>';
    html += '</thead>';
    $('#example1').html(html);
    if ($.fn.dataTable.isDataTable('#example1')) {
        table.destroy();
    }
    table = $('#example1').DataTable({
        "responsive": true,
        "proccessing": true,
        "serverSide": true,
        "searching": true,
        "paging": true,
        "info": false,
        fixedHeader: {
            header: true
        },
        order: [[0, 'asc']],
        "ajax": {
            url: "/KpiReport/UserActions",
            type: 'POST'
        },
        "language": {
            "search": "",
            "searchPlaceholder": "Search..."
        },
        "columns": [
             { "data": "Name" },
             { "data": "Email" },
             { "data": "Action" },
             { "data": "StartDate" }
        ]
    });
}

//get user action chart
function getUserActionChart() {
    $.ajax({
        url: "/api/Chart/useractions",
        async: true,
        success: function (data) {
            //console.log(data);
            var lineChartCanvas = $('#lineChart1').get(0).getContext('2d');

            var areaChartData = {};
            var xAxisArr = [];
            var yAxisAndroidArr = [];
            var bgColor = [];
            var dynamicColors = function () {
                var r = Math.floor(Math.random() * 255);
                var g = Math.floor(Math.random() * 255);
                var b = Math.floor(Math.random() * 255);
                return "rgba(" + r + "," + g + "," + b + ",0.5)";
            };
            for (var i = 0; i < data.length; i++) {
                xAxisArr.push(data[i].xAxis);
                yAxisAndroidArr.push(data[i].yAxisAndroid);
                bgColor.push(dynamicColors());
            }

            var myPieChart = new Chart(lineChartCanvas, {
                type: 'pie',
                data: {
                    datasets: [{
                        data: yAxisAndroidArr,
                        backgroundColor: bgColor
                    }],
                    // These labels appear in the legend and in the tooltips when hovering different arcs
                    labels: xAxisArr
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: true,
                    elements: {
                        arc: {
                            borderWidth: 0
                        }
                    }
                }
            });
        }
    });
}

//get user promo code chart
function getUserPromoCodeChart() {
    $.ajax({
        url: "/api/Chart/userpromocodes",
        async: true,
        success: function (data) {
            //console.log(data);
            var lineChartCanvas = $('#lineChart2').get(0).getContext('2d');

            var areaChartData = {};
            var xAxisArr = [];
            var yAxisAndroidArr = [];
            var bgColor = [];
            var dynamicColors = function () {
                var r = Math.floor(Math.random() * 255);
                var g = Math.floor(Math.random() * 255);
                var b = Math.floor(Math.random() * 255);
                return "rgba(" + r + "," + g + "," + b + ",0.5)";
            };

            for (var i = 0; i < data.length; i++) {
                xAxisArr.push(data[i].xAxis);
                yAxisAndroidArr.push(data[i].yAxisAndroid);
                bgColor.push(dynamicColors());
            }

            var myPieChart = new Chart(lineChartCanvas, {
                type: 'pie',
                data: {
                    datasets: [{
                        data:yAxisAndroidArr,
                        backgroundColor: bgColor
                    }],
                    // These labels appear in the legend and in the tooltips when hovering different arcs
                    labels: xAxisArr
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: true,
                    elements: {
                        arc: {
                            borderWidth: 0
                        }
                    }
                }
            });
        }
    });
}

//get user plan chart
function getUserPlanChart() {
    $.ajax({
        url: "/api/Chart/userplans",
        async: true,
        success: function (data) {
            //console.log(data);
            var lineChartCanvas = $('#lineChart3').get(0).getContext('2d');
            var lineChartOptions = areaChartOptions;

            var areaChartData = {};
            var xAxisArr = [];
            var yAxisAndroidArr = [];
            var bgColor = [];
            var dynamicColors = function () {
                var r = Math.floor(Math.random() * 255);
                var g = Math.floor(Math.random() * 255);
                var b = Math.floor(Math.random() * 255);
                return "rgba(" + r + "," + g + "," + b + ",0.5)";
            };
            for (var i = 0; i < data.length; i++) {
                xAxisArr.push(data[i].xAxis);
                yAxisAndroidArr.push(data[i].yAxisAndroid);
                bgColor.push(dynamicColors());
            }
            areaChartData = {
                labels: xAxisArr,
                datasets: [
                    {
                        label: 'User Plan Usage',
                        borderColor: bgColor,
                        backgroundColor: bgColor,
                        fill: false,
                        data: yAxisAndroidArr
                    }
                ]
            }
            Chart.Bar(lineChartCanvas, {
                data: areaChartData,
                options: lineChartOptions
            });
        }
    });
}

function ResetPasswordPop(userId) {
    var url = "/User/ResetPassword?id=" + userId;
    $("#CustomModalContentDiv").load(url, function () {
        $("#CustomModal").modal("show");
    });
}

function MoreInfo(userId) {
    var url = "/User/UserInfo?id=" + userId;
    $("#CustomModalContentDiv").load(url, function () {
        $("#CustomModal").modal("show");
    });
    //if ($("#selectReport").val() == "All Users") {
    //    getUsersList();
    //}
    //else if ($("#selectReport").val() == "Users by Country") {
    //    getUsersByCountryList();
    //}
    //else if ($("#selectReport").val() == "Active Users") {
    //    getActiveUsersList();
    //}
    //else if ($("#selectReport").val() == "In-app Purchases") {
    //    getInAppUsersList();
    //}
    //else if ($("#selectReport").val() == "Gross Revenue") {
    //    getGrossAmountUsersList();
    //}
    //else if ($("#selectReport").val() == "Total Revenue") {
    //    getTotalAmountUsersList();
    //}
    //else if ($("#selectReport").val() == "Promotions") {
    //    getPromoCodeUsersList();
    //}
    //else if ($("#selectReport").val() == "Perks") {
    //    getPerksUsersList();
    //}
    //else if ($("#selectReport").val() == "Perks Redemption") {
    //    getRedeemUsersList();
    //}
}

function SentEmailPop() {
    var sel = table.rows({ selected: true })[0];
    var users = [];

    for (var i = 0; i < sel.length; i++) {
        users.push($("table tbody tr:eq(" + i + ") td:eq(5)").text().trim());
    }

    var obj = { obj: users };

    $.ajax({
        type: "POST",
        url: "/User/SendEmail",
        data: obj,
        dataType: "html",
        success: function (d) {
            $("#CustomModalContentDiv").html(d);
            $("#CustomModal").modal("show");
        }
    });
}

function SentNotificationPop() {
    var sel = table.rows({ selected: true })[0];
    var users = [];

    for (var i = 0; i < sel.length; i++) {
        users.push($("table tbody tr:eq(" + i + ") td:eq(5)").text().trim());
    }

    var obj = { obj: users };

    $.ajax({
        type: "POST",
        url: "/User/PushNotification",
        data: obj,
        dataType: "html",
        success: function (d) {
            $("#CustomModalContentDiv").html(d);
            $("#CustomModal").modal("show");
        }
    });
}

function ChangeNotificationSetting(value) {
    if (value == "1") value = "0";
    else value = "1";

    $.ajax({
        type: "GET",
        url: "/User/ChangeNotificationSetting?value=" + value,
        data: {},
        success: function (d) {
            var msg = "Reminder notification setting modified successfully.";
            showMessage("success", msg);
            $("#reminder-notification").load("/User/ReminderSetting");
        }
    });
}