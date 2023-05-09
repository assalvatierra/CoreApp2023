
/*
 *  Handle Activity History Modal 
 *  Display and display in a timeline the list of 
 *  activites based on the code selected
 */ 


function SearchCodeActivities(code) {
    $.get("/Activities/GetCodeHistoryList", { salesCode: code }, (result) => {

        $("#SearchCode-Code").text(code);
        console.log(result);

        $("#SearchCode-Activities").children().remove();


        for (var i = 0; i < result.length; i++) {
            var remarks = result[i]["Remarks"] == null ? "" : result[i]["Remarks"] ;
            var projectName = result[i]["ProjectName"] == null ? " " : result[i]["ProjectName"] ;
            var Date = moment( result[i]["Date"] ).format("MMM DD YYYY");
            var amount = result[i]["Amount"].toLocaleString(); 

            $("#SearchCode-Company").text(result[i]["Name"]);
            $("#SearchCode-Amount").text("Amount : " + amount);
            $("#SearchCode-Project").text(projectName);

            var activity = "<a href='/Activities/SearchActivities?srchCode=" + code + "' class='list-group-item'> "
                + "<div class='Activity-Timeline-Side'> </div><div class='blue-circle-history'></div> "
                + Date + " - " 
                + " <span class='label label-default'>" + result[i]["Status"] + "</span> - " + result[i]["ActivityType"]
                + " | <span class='text-muted'> " + remarks + " </span> ";
          
            $("#SearchCode-Activities").append(activity);
        }

        $("#SearchCodeModal").modal("show");
    });
}

function SearchCodeSupActivities(code) {
    $.get("/Activities/GetSupCodeHistoryList", { salesCode: code }, (result) => {
        console.log(result);
        $("#SearchCode-Code").text(code);

        $("#SearchCode-Activities").children().remove();

        for (var i = 0; i < result.length; i++) {
            var remarks = result[i]["Remarks"] == null ? "" : result[i]["Remarks"];
            var projectName = result[i]["ProjectName"] == null ? " " : result[i]["ProjectName"];
            var Date = moment(result[i]["Date"]).format("MMM DD YYYY");
            var amount = result[i]["Amount"].toLocaleString(); 

            $("#SearchCode-Company").text(result[i]["Name"]);
            $("#SearchCode-Project").text(projectName);
            $("#SearchCode-Amount").text(" ");

            var activity = "<a href='/Activities/SearchActivities?srchCode=" + code + "' class='list-group-item'> "
                + "<div class='Activity-Timeline-Side'> </div> <div class='blue-circle-history'></div> "
                + Date + " - "
                + " <span class='label label-default'>" + result[i]["Status"] + "</span> - " + result[i]["ActivityType"]
                + " | <span class='text-muted'> " + remarks + " </span> "
                + " <div class='Activity-Timeline-Amount'>  Amount: " + amount + " </div>";

            $("#SearchCode-Activities").append(activity);
        }

        $("#SearchCodeModal").modal("show");
    });
}

function ShowSearchCodeModal() {
    $("#SearchCodeModal").modal("show");
}
