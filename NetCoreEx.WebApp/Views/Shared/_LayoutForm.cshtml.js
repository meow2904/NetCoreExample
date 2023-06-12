/* BEGIN EXTERNAL SOURCE */
/* END EXTERNAL SOURCE */
/* BEGIN EXTERNAL SOURCE */
var objAjax = {
    parameters: undefined,
    isFuntConfirm: false,
    isPageAdmin: true,
    divLoading: undefined
};
/* END EXTERNAL SOURCE */
/* BEGIN EXTERNAL SOURCE */
/* END EXTERNAL SOURCE */
/* BEGIN EXTERNAL SOURCE */
/* END EXTERNAL SOURCE */
/* BEGIN EXTERNAL SOURCE */
/* END EXTERNAL SOURCE */
/* BEGIN EXTERNAL SOURCE */
/* END EXTERNAL SOURCE */
/* BEGIN EXTERNAL SOURCE */
/* END EXTERNAL SOURCE */
/* BEGIN EXTERNAL SOURCE */
/* END EXTERNAL SOURCE */
/* BEGIN EXTERNAL SOURCE */
/* END EXTERNAL SOURCE */
/* BEGIN EXTERNAL SOURCE */
//Đăng xuất hệ thống
function LogOut() {
    window.objAjax.parameters = undefined;
    window.objAjax.isFuntConfirm = true;
    window.objAjax.divLoading = "body";
    AjaxGetAsync("/Api/Employee/Logout", window.objAjax, function (data) {
        window.location.href = "/Login";
    });
}
/* END EXTERNAL SOURCE */
//# sourceMappingURL=_LayoutForm.cshtml.js.map