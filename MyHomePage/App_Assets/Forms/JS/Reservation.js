$(document).ready(function () {
    $('#kRoomSpan').hide();
    $('#kTrainSpan').hide();
    $('#networkWarn').hide();

    $("input[name='presRadio'][value=Yes]").prop('checked', true);
    $('#equipment_0').prop('checked', true);
    $('#equipment_1').prop('checked', true);
    $('#equipment_3').prop('checked', true);
});

$('#presRadio input').change(function () {
    if ($(this).prop('id') !== "presRadio_1") {
        $('#equipment_0').prop('checked', true);
        $('#equipment_1').prop('checked', true);
        $('#equipment_3').prop('checked', true);
    } else {
        $('#equipment input:checked').each(function () {
            $(this).prop('checked', false);
        });
    }
});

// Fill out first and last name fields automatically after email input
$("#email").focusout(function () {
    var fullname = $("#email").val();
    var first = "", last = "", tmp = "";
    var breakpoint = false, firstChar1 = true, firstChar2 = true;

    for (var i = 0; i < fullname.length; i++) {
        if (fullname[i] !== ".") {
            if (breakpoint) {
                if (firstChar2) {
                    tmp = fullname[i];
                    last += tmp.toUpperCase();
                    firstChar2 = false;
                } else {
                    last += fullname[i];
                }
            } else {
                if (firstChar1) {
                    tmp = fullname[i]
                    first += tmp.toUpperCase();
                    firstChar1 = false;
                } else {
                    first += fullname[i];
                }
            }
        } else {
            breakpoint = true;
        }
    }
    $("#fname").val(first).change();
    $("#lname").val(last).change();
});

// Add Chosen styles and add select functions
$("#epaRooms").chosen({ disable_search_threshold: 5 }).change(function () {
    if ($(this).val() == "") {
        $('#building').prop('disabled', false).trigger('chosen:updated');
        $("#epaTraining").prop('disabled', false).trigger('chosen:updated');
        $("#epaConf").prop('disabled', false).trigger('chosen:updated');
        checkNetwork($(this).val());
    } else {
        $('#building').prop('disabled', true).trigger('chosen:updated');
        $("#epaTraining").prop('disabled', true).trigger('chosen:updated');
        $("#epaConf").prop('disabled', true).trigger('chosen:updated');
        checkNetwork($(this).val());
    }
});

$("#epaTraining").chosen({ disable_search_threshold: 5 }).change(function () {
    if ($(this).val() == "") {
        $('#building').prop('disabled', false).trigger('chosen:updated');
        $("#epaRooms").prop('disabled', false).trigger('chosen:updated');
        $("#epaConf").prop('disabled', false).trigger('chosen:updated');
        checkNetwork($(this).val());
    } else {
        $('#building').prop('disabled', true).trigger('chosen:updated');
        $("#epaRooms").prop('disabled', true).trigger('chosen:updated');
        $("#epaConf").prop('disabled', true).trigger('chosen:updated');
        checkNetwork($(this).val());
    }
});

$("#kRooms").chosen({ disable_search_threshold: 5 }).change(function () {
    if ($(this).val() == "") {
        $('#building').prop('disabled', false).trigger('chosen:updated');
        $("#kTraining").prop('disabled', false).trigger('chosen:updated');
        checkNetwork($(this).val());
    } else {
        $('#building').prop('disabled', true).trigger('chosen:updated');
        $("#kTraining").prop('disabled', true).trigger('chosen:updated');
        checkNetwork($(this).val());
    }
});

$("#kTraining").chosen({ disable_search_threshold: 5 }).change(function () {
    if ($(this).val() == "") {
        $('#building').prop('disabled', false).trigger('chosen:updated');
        $("#kRooms").prop('disabled', false).trigger('chosen:updated');
        checkNetwork($(this).val());
    } else {
        $('#building').prop('disabled', true).trigger('chosen:updated');
        $("#kRooms").prop('disabled', true).trigger('chosen:updated');
        checkNetwork($(this).val());
    }
});

$("#epaConf").chosen({ disable_search_threshold: 5 }).change(function () {
    if ($(this).val() == "") {
        $('#building').prop('disabled', false).trigger('chosen:updated');
        $("#epaRooms").prop('disabled', false).trigger('chosen:updated');
        $("#epaTraining").prop('disabled', false).trigger('chosen:updated');
        checkNetwork($(this).val());
    } else {
        $('#building').prop('disabled', true).trigger('chosen:updated');
        $("#epaRooms").prop('disabled', true).trigger('chosen:updated');
        $("#epaTraining").prop('disabled', true).trigger('chosen:updated');
        checkNetwork($(this).val());
    }
});

$("#building").chosen({ disable_search_threshold: 5 }).change(function () {
    if ($(this).val() == "") {
        $("#epaRooms").prop('disabled', true).trigger('chosen:updated');
        $("#epaTraining").prop('disabled', true).trigger('chosen:updated');
        $("#kRooms").prop('disabled', true).trigger('chosen:updated');
        $("#kTraining").prop('disabled', true).trigger('chosen:updated');
        $("#epaConf").prop('disabled', true).trigger('chosen:updated');
        checkNetwork($(this).val());
    } else {
        $("#epaRooms").prop('disabled', false).trigger('chosen:updated');
        $("#epaTraining").prop('disabled', false).trigger('chosen:updated');
        $("#kRooms").prop('disabled', false).trigger('chosen:updated');
        $("#kTraining").prop('disabled', false).trigger('chosen:updated');
        $("#epaConf").prop('disabled', false).trigger('chosen:updated');
    }
});;

// Add timepicker to meeting time selections
$("#startTime").timepicker({
    'scrollDefault': '12:00pm',
    'minTime': '5:00am',
    'maxTime': '5:00pm',
    'forceRoundTime': true
});

$("#startTime").on('changeTime', function () {
    $("#endTime").removeClass('disabled');
    $("#endTime").timepicker({
        'minTime': $("#startTime").val(),
        'maxTime': '6:00pm',
        'forceRoundTime': true,
        'showDuration': true
    });
});

$("#endTime").on('changeTime', function () {
    $("#endTimeLbl").text($(this).val());
})

// Dropdown dynamics
$("#building").change(function () {
    var building = $(this).val();
    if (building == 'epa') {
        // Enabled all EPA and disable 801k
        $('#kRoomSpan').hide();
        $('#kTrainSpan').hide();
        $('#epaRoomSpan').show();
        $('#epaTrainSpan').show();
        $("#epaRooms").prop('disabled', false).trigger("chosen:updated");
        $("#epaTraining").prop('disabled', false).trigger("chosen:updated");
        $("#epaConf").prop('disabled', false).trigger("chosen:updated");
    } else if (building == '801k') {
        // Enable all 801k and disable EPA
        $("#epaConf").prop('disabled', true).trigger("chosen:updated");
        $('#epaRoomSpan').hide();
        $('#epaTrainSpan').hide();
        $('#kRoomSpan').show();
        $('#kTrainSpan').show();
    }
});

// Network alerts
function checkNetwork(room) {
    var recycleNetworkEnabled = ['110', '220', '230', '810', '910', '1010', '1310', '1910', '2210', '2310',
        '2410', '2510', '2520', '2530', 'Training 18', '1 East', '1 West', '2 East', '2 West', 'Byron Sher',
        'Coastal', 'Sierra', '1502', '1702', '1902 (Sue Case)', '1917', '1919'];
    var foundNetwork = false;
    if (room == "") {
        $('#networkWarn').hide();
        $('#networkReq_0').prop('disabled', false);
        $('#networkReq_0').prop('checked', false);
        $('#networkReq_1').prop('checked', false);
    } else {
        for (var i = 0; i < recycleNetworkEnabled.length; i++) {
            if (room == recycleNetworkEnabled[i]) {
                $('#networkWarn').hide();
                $('#networkReq_0').prop('disabled', false);
                $('#networkReq_1').prop('checked', false);
                foundNetwork = true;
                break;
            }
        }
        if (foundNetwork == false) {
            $('#networkWarn').show();
            $('#networkReq_0').prop('disabled', true);
            $('#networkReq_0').prop('checked', false);
            $('#networkReq_1').prop('checked', true);
        }
    }
}