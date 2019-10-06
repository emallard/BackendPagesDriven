function renderForm2(form, h, r) {

    r.afterRender(() => document.getElementById(h).addEventListener('submit', (evt) => {
        evt.preventDefault();

        applyInputsToModel(r.page);
        applyOnSubmits(r.page);
        console.log('submit ' + h);
        formCall(form);
        return false;
    }));


    var hForm = pathWithoutPage(h);
    var pageName = getPageName(h);

    var command = form.Command;

    let inputFields = getInputsToBeRendered(r.page, command, hForm + '.Command');
    console.log('InputsToBeRendered : \n  ' + inputFields.join('\n  '));

    let html = '';
    for (let hField of inputFields) {
        let fieldValue = getValue(r.page, hField.split('.'));
        let input2 = createInput(r.page, form, hForm, fieldValue, hField);
        html += input2.render(fieldValue, pageName + '.' + hField, r);
        r.afterRender(() => {
            input2.updateFromModel()
        });
    }

    return `<form id="${h}">
                ${html}
                <button type="submit" class="btn btn-primary">${field(h)}</button>
            </form>`;
}

function getInputsToBeRendered(page, command, hCommand) {

    let hInputs = [];
    for (let k of Object.keys(command)) {

        let commandField = hCommand + '.' + k;

        let foundOnSubmit = false;
        for (let onSubmit of page.OnSubmits) {

            let onSubmitCommandField = onSubmit.To.join('.');
            if (onSubmitCommandField == commandField) {
                hInputs.push(onSubmit.From.slice(0, 1).join('.'));
                foundOnSubmit = true;
            }
        }
        if (!foundOnSubmit) {
            hInputs.push(commandField);
        }
    }
    return hInputs;
}

var _pageinputs = [];
function createInput(page, form, hForm, field, hField) {

    let typeName = null;
    let hCommand = hForm + '.Command';
    if (hField.startsWith(hCommand)) {
        let fieldName = hField.split('.')[hField.split('.').length - 1];
        typeName = form['CommandFieldTypeNames'][fieldName];
    }

    var input = selectInput(field, typeName);
    input.hModel = hField;

    _pageinputs.push(input);
    return input;
}

function selectInput(x, typeName) {
    if (typeName != null) {
        if (typeName == 'string')
            return new GenericInput('');
        if (typeName == 'double')
            return new GenericInput('number');
        if (typeName == 'DateTime')
            return new GenericInput('date');
        //if (typeName.startsWith('ID<'))
        //    return new GenericInput();
    }
    if (x["IsSelect"])
        return new SelectInput();

    return new GenericInput();
}