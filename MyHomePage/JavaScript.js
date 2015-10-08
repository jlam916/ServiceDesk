function hideDiv(checkbox, div) {
    if (checkbox.checked) {
        document.getElementById(div.id).style.display = 'none';
    }
    else {
        document.getElementById(div.id).style.display = '';
    }
}

function showDiv(div) {
    document.getElementById(div.id).style.display = '';
}

function collapse() {
    var inputs = document.getElementsByTagName("input");
    var parent, siblings, divToHide;

    for (var i = 0; i < inputs.length; i++) {
        if (inputs[i].type == "checkbox") {
            inputs[i].checked = true;
            parentDiv = inputs[i].parentNode.parentNode;
            siblings = parentDiv.childNodes;

            for (var j = 0; j < siblings.length; j++) {
                if (siblings[j].tagName == "DIV") {
                    divToHide = siblings[j];
                    break;
                }
            }

            hideDiv(inputs[i], divToHide);
        }
    }
}

function expand() {
    var inputs = document.getElementsByTagName("input");
    var parent, siblings, divToShow;

    for (var i = 0; i < inputs.length; i++) {
        if (inputs[i].type == "checkbox") {
            inputs[i].checked = false;
            parentDiv = inputs[i].parentNode.parentNode;
            siblings = parentDiv.childNodes;

            for (var j = 0; j < siblings.length; j++) {
                if (siblings[j].tagName == "DIV") {
                    divToShow = siblings[j];
                    break;
                }
            }

            showDiv(divToShow);
        }
    }
}