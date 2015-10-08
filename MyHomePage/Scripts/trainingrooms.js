$(document).ready(function () {
    $('#loadModal').on('shown.bs.modal', function (e) {
        window.setTimeout(updatePage, 2000);
    })

    $('.north').click(function () {
        // Remove green class
        var id = getActiveID("north");
        $('#' + id).removeClass("btn-success");;
        // Make pressed button green
        $('#' + this.id).removeClass('btn-default');
        $('#' + this.id).addClass('btn-success');
    })

    $('.south').click(function () {
        // Remove green class
        var id = getActiveID("south");
        $('#' + id).removeClass("btn-success");
        // Make pressed button green
        $('#' + this.id).removeClass('btn-default');
        $('#' + this.id).addClass('btn-success');
    })

    $('.dropdown-toggle').dropdown()
});

function updatePage() {
    $('#loadModal').modal('hide');
}