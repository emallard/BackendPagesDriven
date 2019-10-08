function renderForm2(form, id, r) {

    r.afterRender(() => document.getElementById(id).addEventListener('submit', (evt) => {
        evt.preventDefault();

        r.applyInputsToModel();
        r.applyOnSubmits();
        console.log('submit ' + id);
        formCall(form);
        return false;
    }));

    var command = form.Command;

    let inputFields = getInputsToBeRendered(r.page, command, id + '.Command');
    console.log('InputsToBeRendered : \n  ' + inputFields.join('\n  '));

    let html = '';
    for (let idField of inputFields) {
        let fieldValue = getValue(r.page, idField.split('.'));
        let input2 = createInput(form, id, fieldValue, idField, r);
        html += input2.render(fieldValue, idField, r);
        r.afterRender(() => {
            input2.updateFromModel()
        });
    }

    return `<form id="${id}">
                ${html}
                <button type="submit" class="btn btn-primary">${field(id)}</button>
            </form>`;
}

function getInputsToBeRendered(page, command, hCommand) {

    let idInputs = [];
    for (let k of Object.keys(command)) {

        let commandField = hCommand + '.' + k;

        let foundOnSubmit = false;
        for (let onSubmit of page.OnSubmits) {

            let onSubmitCommandField = onSubmit.To.join('.');
            if (onSubmitCommandField == commandField) {
                idInputs.push(onSubmit.From.slice(0, 1).join('.'));
                foundOnSubmit = true;
            }
        }
        if (!foundOnSubmit) {
            idInputs.push(commandField);
        }
    }
    return idInputs;
}

function createInput(form, idForm, field, idField, r) {

    let typeName = null;
    let hCommand = idForm + '.Command';
    if (idField.startsWith(hCommand)) {
        let fieldName = idField.split('.')[idField.split('.').length - 1];
        typeName = form['CommandFieldTypeNames'][fieldName];
    }

    var input = selectInput(field, typeName);

    r.inputs.push(input);
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