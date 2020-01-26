$(document).ready(function () {
    var selectedEmployeedId = $("#employeeList").find("option:selected").val();
    $('#employeeList').find('option').remove();
    $('#employeeList')
        .append($("<option></option>")
            .attr("value", selectedEmployeedId)
            .text(selectedEmployeedId));

    $("#projectList").change(function () {
        var selectedProjectId = $(this).children("option:selected").val();
        $.ajax({
            type: "POST",
            url: "/WorkItems/GetEmployeesFromProject",
            dataType: "json",
            data: { "project_id": selectedProjectId} ,
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

    });

});