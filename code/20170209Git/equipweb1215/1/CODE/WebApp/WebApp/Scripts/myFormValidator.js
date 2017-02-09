function myValidation() {

    if ($('#cjname').length > 0 && ($('#cjname').find("option:selected").val() == null || $('#cjname').find("option:selected").val() == "")) {
        alert('请选择车间！');
        return false;
    }

    if ($('#zzId').length > 0 && ($('#zzId').find("option:selected").val() == null || $('#zzId').find("option:selected").val() == "")) {
        alert('请选择装置！');
        return false;
    }

    if ($('#Equip_GyCode').length > 0 && ($('#Equip_GyCode').find("option:selected").val() == null || $('#Equip_GyCode').find("option:selected").val() == "")) {
        alert('请选择设备工艺编号！');
        return false;
    }

    //if ($('#E_Code').length > 0 && ($('#E_Code').val() == null || $('#E_Code').val() == "")) {
    //    alert('请输入设备编号！');
    //    return false;
    //}

    //if ($('#E_Type').length > 0 && ($('#E_Type').val() == null || $('#E_Type').val() == "")) {
    //    alert('请输入设备型号！');
    //    return false;
    //}
    if ($("input:radio[name='zyoptionsRadiosinline']").length > 0 && ($("input:radio[name='zyoptionsRadiosinline']:checked").val() == null || $("input:radio[name='zyoptionsRadiosinline']:checked").val() == "")) {
        alert('请选择问题专业分类！');
        return false;
    }
    //由于其他专业的设备可能无专业分类子类，因此注释这一段
    //if ($("input:radio[name='subzyoptionsRadiosinline']").length > 0 && ($("input:radio[name='subzyoptionsRadiosinline']:checked").val() == null || $("input:radio[name='subzyoptionsRadiosinline']:checked").val() == "")) {
    //    alert('请选择专业分类子类！');
    //    return false;
    //}

    if ($("input:radio[name='srcoptionsRadiosinline']").length > 0 && ($("input:radio[name='srcoptionsRadiosinline']:checked").val() == null || $("input:radio[name='srcoptionsRadiosinline']:checked").val() == "")) {
        alert('请选择数据来源！');
        return false;
    }

    if (($("textarea.form-control").length > 0 && ($("textarea.form-control:first").val() == null || $("textarea.form-control:first").val() == "")) && ($("#fileUpLoad_name").length > 0 && ($("#fileUpLoad_name").val() == null || $("#fileUpLoad_name").val() == ""))) {
        alert('描述或附件，两者不能同时为空，请至少输入其中一项！');
        return false;
    }

    //if ($("#fileUpLoad_name").length > 0 && ($("#fileUpLoad_name").val() == null || $("#fileUpLoad_name").val() == "")) {
    //    alert('请上传附件！');
    //    return false;
    //}

    if ($("#EquipMaintain_Time").length > 0 && ($("#EquipMaintain_Time").val() == null || $("#EquipMaintain_Time").val() == "")) {
        alert('请输入设备维护时间！');
        return false;
    }

    return true;

};