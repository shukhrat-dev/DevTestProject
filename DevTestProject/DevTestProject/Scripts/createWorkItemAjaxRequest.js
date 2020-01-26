$(document).ready(function () {
    var selectedProjectId = $("#projectList").children("option:selected").val();
    console.log(selectedProjectId);
    ajaxRequest(selectedProjectId);

    $("#projectList").change(function () {
        var selectedProjectId = $(this).children("option:selected").val();
        ajaxRequest(selectedProjectId);

    });
        

});





function ajaxRequest(selectedProjectId) {
    $.ajax({
        type: "POST",
        url: "/WorkItems/GetEmployeesFromProject",
        dataType: "json",
        data: { "project_id": selectedProjectId },
        success: function (data) {
            $('#employeeList').find('option').remove();
            for (key in data) {
                for (var i = 0; i < data[key].length; i += 1) {
                    var dataValue = data[key][i];
                    $('#employeeList')
                        .append($("<option></option>")
                            .attr("value", dataValue)
                            .text(dataValue));
                }
            }
        },
        error: function (data) {
            alert('error');
            console.log(data);
        }
    });
}