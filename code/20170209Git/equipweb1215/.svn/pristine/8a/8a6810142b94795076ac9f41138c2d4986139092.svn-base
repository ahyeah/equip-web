

function DispalsySide(Entity_Ser) {
    //显示右边的导航条


    var inhtml;
    $.ajax({
        url: '/Common/WorkFlow_MainProcess',
        dataType: 'json',
        type: 'post',
        data: { "Entity_Ser": Entity_Ser },
        success: function (backdata) {
            inhtml = "<h5 >设备工艺编号：<a  role=\"button\" data-toggle=\"modal\" data-target=\"#EquipInfoModal\"  onclick=\"displayEquipInfo('" + backdata.FLow_EquipGyCode + "')\"> " + backdata.FLow_EquipGyCode + "</a></h5>";
            inhtml = inhtml + "<ul class=\"timeline\"> <li class=\"time-label\">  <span class=\"bg-red\">" + backdata.Flow_Name + "</span>";
            //inhtml = inhtml + "<div class=\"timeline-item\">";
            inhtml = inhtml + "<a href=\"" + backdata.FLow_DescPath + "\" target=_blank>" + "流程说明</a></span>";
            inhtml = inhtml + "</li>";
            for (var i = 0; i < backdata.Miss.length; i++) {
                if ((backdata.Miss[i]).Miss_Type == 0) {
                    inhtml = inhtml + "<li> <i class=\"fa fa-user bg-aqua\"></i>";
                    inhtml = inhtml + "<div class=\"timeline-item\">"
                    inhtml = inhtml + "<span class=\"time\"><i class=\"fa fa-clock-o\"></i>" + (backdata.Miss[i]).Miss_Time + "</span>";
                    //inhtml = inhtml + "<h1 class=\"timeline-header no-border\"><a  data-toggle=\"modal\" data-target=\"#ParamModal\"  onclick=\"displayParams('" + backdata.Flow_Id + "','" + (backdata.Miss[i]).Miss_Id + "')\"> " + (backdata.Miss[i]).Miss_Name + "</a></h1>";
                    inhtml = inhtml + "<h5  class=\"timeline-header no-border\" role=\"button\" data-toggle=\"modal\" data-target=\"#ParamModal\"  onclick=\"displayParams('" + backdata.Flow_Id + "','" + (backdata.Miss[i]).Miss_Id + "')\"> " + (backdata.Miss[i]).Miss_Name + "</h5>";
                    inhtml = inhtml + " </div>   </li>"
                }
                else if ((backdata.Miss[i]).Miss_Type == 2) {
                    inhtml = inhtml + "<li> <i class=\"fa fa-user bg-aqua\"></i>";
                    inhtml = inhtml + "<div class=\"timeline-item\">"
                    inhtml = inhtml + "<span class=\"time\"><i class=\"fa fa-clock-o\"></i>待处理任务</span>";
                    //inhtml = inhtml + "<h1 class=\"timeline-header no-border\"><a  data-toggle=\"modal\" data-target=\"#ParamModal\"  onclick=\"displayParams('" + backdata.Flow_Id + "','" + (backdata.Miss[i]).Miss_Id + "')\"> " + (backdata.Miss[i]).Miss_Name + "</a></h1>";
                    inhtml = inhtml + "<h5 class=\"timeline-header no-border\" > " + (backdata.Miss[i]).Miss_Name + "</h5>";
                    inhtml = inhtml + " </div>   </li>"
                }


                else {
                    if (backdata.Miss[i].Miss_LinkFlowType == "parallel") {
                        inhtml = inhtml + "<li> <i class=\"fa fa-user bg-aqua\"></i>";
                        inhtml = inhtml + "<div class=\"timeline-item\">";
                        //inhtml = inhtml + "<span class=\"time\"><i class=\"fa fa-clock-o\"></i> " + (backdata.Miss[i]).Miss_Time + "</span>";
                        inhtml = inhtml + "<span class=\"time\"><a href=\"" + (backdata.Miss[i]).Miss_LinkFlowName + "\" target=_blank>" + "流程说明</a></span>";
                        if ((backdata.Miss[i]).Miss_LinkFlowId > -1)
                            inhtml = inhtml + "<a class=\"btn btn-primary btn-warning\" role=\"button\" data-toggle=\"collapse\" href=\"#FLow_" + (backdata.Miss[i]).Miss_LinkFlowId + "\" aria-expanded=true aria-controls=FLow_" + (backdata.Miss[i]).Miss_LinkFlowId + " onclick=\"subFlow(" + (backdata.Miss[i]).Miss_LinkFlowId + ")\">" + (backdata.Miss[i]).Miss_Name + "</a>";
                        else
                            inhtml = inhtml + "<a class=\"btn btn-primary btn-warning\" role=\"button\" data-toggle=\"collapse\" >" + (backdata.Miss[i]).Miss_Name + "</a>";
                        inhtml = inhtml + " <div class=\"collapse\" id=FLow_" + (backdata.Miss[i]).Miss_LinkFlowId + ">";

                        inhtml = inhtml + "</div> </li>";


                    }
                    if (backdata.Miss[i].Miss_LinkFlowType == "serial") {
                        // inhtml = inhtml + "<li> <i class=\"fa fa-user bg-aqua\"></i>";
                        // inhtml = inhtml + "<div class=\"timeline-item\">"
                        // inhtml = inhtml + "<span class=\"time\"><i class=\"fa fa-clock-o\"></i>" + (backdata.Miss[i]).Miss_Time + "</span>";
                        // inhtml = inhtml + "<h1 class=\"timeline-header no-border\"><a href=\"/A13dot1/WorkFlow_HisDetailParallel?Entity_Id=" + (backdata.Miss[i]).Miss_LinkFlowId + "\" target=\"_blank\">" + backdata.Miss[i].Miss_Name + "</a> </h1>";
                        // inhtml = inhtml + " </div>   </li>"
                        inhtml = inhtml + "<li> <i class=\"fa fa-user bg-aqua\"></i>";
                        inhtml = inhtml + "<div class=\"timeline-item\">";
                        // inhtml = inhtml + "<span class=\"time\"><i class=\"fa fa-clock-o\"></i> " + (backdata.Miss[i]).Miss_Time + "</span>";
                        inhtml = inhtml + "<span class=\"time\"><a href=\"" + (backdata.Miss[i]).Miss_LinkFlowName + "\" target=_blank>" + "流程说明</a></span>";
                        if ((backdata.Miss[i]).Miss_LinkFlowId > -1)
                            inhtml = inhtml + "<a class=\"btn btn-primary btn-success \" role=\"button\"  data-toggle=\"collapse\" href=\"#FLow_" + (backdata.Miss[i]).Miss_LinkFlowId + "\" aria-expanded=true aria-controls=FLow_" + (backdata.Miss[i]).Miss_LinkFlowId + " onclick=\"subFlow(" + (backdata.Miss[i]).Miss_LinkFlowId + ")\">" + (backdata.Miss[i]).Miss_Name + "</a>";
                        else
                            inhtml = inhtml + "<a class=\"btn btn-primary btn-success\" role=\"button\" data-toggle=\"collapse\" >" + (backdata.Miss[i]).Miss_Name + "</a>";
                        inhtml = inhtml + " <div class=\"collapse\" id=FLow_" + (backdata.Miss[i]).Miss_LinkFlowId + ">";

                        inhtml = inhtml + "</div> </li>";


                    }

                }

            }
            inhtml = inhtml + "</ul>";
            $("#sidebar").empty().append(inhtml);
        }
    });



}
function displayParams(Entity_Id, Mission_Id) {

    var inhtml;
    $.ajax({
        url: '/Common/WorkFlow_ListParam',
        dataType: 'json',
        type: 'post',
        data: {
            json1: '{Entity_Id: "' + Entity_Id
                  + '", Mission_Id: "' + Mission_Id
                  + '"}'
        },
        success: function (backdata) {

            inhtml = "<table class=\"table  table-hover table-bordered\"> <tbody><tr>";



            for (var i = 0; i < backdata.length; i++) {

                inhtml = inhtml + " <tr class=\"info\">";
                inhtml = inhtml + "<th>" + backdata[i].Param_Desc + "</th>";
                if (backdata[i].Param_isFile == 1 && backdata[i].Param_Value != "") {


                    inhtml = inhtml + "<td><a href=" + backdata[i].Param_UploadFilePath + " target=_blank>" + backdata[i].Param_SavedFilePath + "</a></td> ";
                }
                else
                    inhtml = inhtml + "<td>" + backdata[i].Param_Value + "</td>";

                inhtml = inhtml + " </tr>";


            }
            inhtml = inhtml + " </tbody></table>";
            $('#ParamDiv').empty().append(inhtml);
        }
    }
    );



}


function displayEquipInfo(Equip_GyCode) {

    var inhtml;
    $.ajax({
        url: '/Common/WorkFlow_EquipInfo',
        dataType: 'json',
        type: 'post',
        data: {
            "Equip_GyCode": Equip_GyCode

        },
        success: function (backdata) {

            inhtml = "<table class=\"table  table-hover table-bordered\"> <tbody><tr>";



            for (var i = 0; i < backdata.length; i++) {

                inhtml = inhtml + " <tr class=\"info\">";
                inhtml = inhtml + "<th>" + backdata[i].Param_Name + "</th>";

                inhtml = inhtml + "<td>" + backdata[i].Param_value + "</td>";

                inhtml = inhtml + " </tr>";


            }
            inhtml = inhtml + " </tbody></table>";
            $('#EquipInfoDiv').empty().append(inhtml);
        }
    }
    );



}

function subFlow(FlowId) {
    var divname = "#FLow_" + FlowId;
    var inhtml;
    $.ajax({
        url: '/Common/WorkFlow_SubProcess',
        dataType: 'json',
        type: 'post',
        data: { "FlowId": FlowId },
        success: function (backdata) {
            inhtml = "<ul class=\"timeline\">";

            for (var i = 0; i < backdata.Miss.length; i++) {
                if ((backdata.Miss[i]).Miss_Type == 0) {
                    inhtml = inhtml + "<li> <i class=\"fa fa-user bg-aqua\"></i>";
                    inhtml = inhtml + "<div class=\"timeline-item\">"
                    inhtml = inhtml + "<span class=\"time\"><i class=\"fa fa-clock-o\"></i>" + (backdata.Miss[i]).Miss_Time + "</span>";
                    //inhtml = inhtml + "<h1 class=\"timeline-header no-border\"><a  data-toggle=\"modal\" data-target=\"#ParamModal\"  onclick=\"displayParams('" + backdata.Flow_Id + "','" + (backdata.Miss[i]).Miss_Id + "')\"> " + (backdata.Miss[i]).Miss_Name + "</a></h1>";
                    inhtml = inhtml + "<h6  class=\"timeline-header no-border\" role=\"button\" data-toggle=\"modal\" data-target=\"#ParamModal\"  onclick=\"displayParams('" + backdata.Flow_Id + "','" + (backdata.Miss[i]).Miss_Id + "')\"> " + (backdata.Miss[i]).Miss_Name + "</h6>";
                    inhtml = inhtml + " </div>   </li>"
                }
                else if ((backdata.Miss[i]).Miss_Type == 2) {
                    inhtml = inhtml + "<li> <i class=\"fa fa-user bg-aqua\"></i>";
                    inhtml = inhtml + "<div class=\"timeline-item\">"
                    inhtml = inhtml + "<span class=\"time\"><i class=\"fa fa-clock-o\"></i>待处理任务</span>";
                    //inhtml = inhtml + "<h1 class=\"timeline-header no-border\"><a  data-toggle=\"modal\" data-target=\"#ParamModal\"  onclick=\"displayParams('" + backdata.Flow_Id + "','" + (backdata.Miss[i]).Miss_Id + "')\"> " + (backdata.Miss[i]).Miss_Name + "</a></h1>";
                    inhtml = inhtml + "<h6  class=\"timeline-header no-border\"> " + (backdata.Miss[i]).Miss_Name + "</h6>";
                    inhtml = inhtml + " </div>   </li>"
                }


                else {
                    if (backdata.Miss[i].Miss_LinkFlowType == "parallel") {
                        inhtml = inhtml + "<li> <i class=\"fa fa-user bg-aqua\"></i>";
                        inhtml = inhtml + "<div class=\"timeline-item\">";
                        //inhtml = inhtml + "<span class=\"time\"><i class=\"fa fa-clock-o\"></i> " + (backdata.Miss[i]).Miss_Time + "</span>";
                        inhtml = inhtml + "<span class=\"time\"><a href=\"" + (backdata.Miss[i]).Miss_LinkFlowName + "\" target=_blank>" + "流程说明</a></span>";
                        if ((backdata.Miss[i]).Miss_LinkFlowId > -1)
                            inhtml = inhtml + "<a class=\"btn btn-primary btn-warning\" role=\"button\" data-toggle=\"collapse\" href=\"#FLow_" + (backdata.Miss[i]).Miss_LinkFlowId + "\" aria-expanded=true aria-controls=FLow_" + (backdata.Miss[i]).Miss_LinkFlowId + " onclick=\"subFlow(" + (backdata.Miss[i]).Miss_LinkFlowId + ")\">" + (backdata.Miss[i]).Miss_Name + "</a>";
                        else
                            inhtml = inhtml + "<a class=\"btn btn-primary btn-warning\" role=\"button\" data-toggle=\"collapse\" >" + (backdata.Miss[i]).Miss_Name + "</a>";
                        inhtml = inhtml + " <div class=\"collapse\" id=FLow_" + (backdata.Miss[i]).Miss_LinkFlowId + ">";

                        inhtml = inhtml + "</div> </li>";


                    }
                    if (backdata.Miss[i].Miss_LinkFlowType == "serial") {
                        // inhtml = inhtml + "<li> <i class=\"fa fa-user bg-aqua\"></i>";
                        // inhtml = inhtml + "<div class=\"timeline-item\">"
                        // inhtml = inhtml + "<span class=\"time\"><i class=\"fa fa-clock-o\"></i>" + (backdata.Miss[i]).Miss_Time + "</span>";
                        // inhtml = inhtml + "<h1 class=\"timeline-header no-border\"><a href=\"/A13dot1/WorkFlow_HisDetailParallel?Entity_Id=" + (backdata.Miss[i]).Miss_LinkFlowId + "\" target=\"_blank\">" + backdata.Miss[i].Miss_Name + "</a> </h1>";
                        // inhtml = inhtml + " </div>   </li>"
                        inhtml = inhtml + "<li> <i class=\"fa fa-user bg-aqua\"></i>";
                        inhtml = inhtml + "<div class=\"timeline-item\">";
                        // inhtml = inhtml + "<span class=\"time\"><i class=\"fa fa-clock-o\"></i> " + (backdata.Miss[i]).Miss_Time + "</span>";
                        inhtml = inhtml + "<span class=\"time\"><a href=\"" + (backdata.Miss[i]).Miss_LinkFlowName + "\" target=_blank>" + "流程说明</a></span>";
                        if ((backdata.Miss[i]).Miss_LinkFlowId > -1)
                            inhtml = inhtml + "<a class=\"btn btn-primary btn-success \" role=\"button\"  data-toggle=\"collapse\" href=\"#FLow_" + (backdata.Miss[i]).Miss_LinkFlowId + "\" aria-expanded=true aria-controls=FLow_" + (backdata.Miss[i]).Miss_LinkFlowId + " onclick=\"subFlow(" + (backdata.Miss[i]).Miss_LinkFlowId + ")\">" + (backdata.Miss[i]).Miss_Name + "</a>";
                        else
                            inhtml = inhtml + "<a class=\"btn btn-primary btn-success\" role=\"button\" data-toggle=\"collapse\" >" + (backdata.Miss[i]).Miss_Name + "</a>";
                        inhtml = inhtml + " <div class=\"collapse\" id=FLow_" + (backdata.Miss[i]).Miss_LinkFlowId + ">";

                        inhtml = inhtml + "</div> </li>";


                    }

                }

            }
            inhtml = inhtml + "</ul>";
            $(divname).empty().append(inhtml);
        }
    }
    );

}