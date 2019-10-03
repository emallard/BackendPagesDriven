function renderForm2(form, h, r) {

    r.afterRender(() => document.getElementById(h).addEventListener('submit', (evt) => {
        evt.preventDefault();

        applyOnSubmits(r.page);

        let keys = Object.keys(form.Command);
        for (let k of keys) {
            form.Command[k] = document.getElementById(h + ".Command." + k).value;
        }

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

    /*

    
    var fieldPathsInForm = [];
    for (let k of Object.keys(command)) {
        fieldPathsInForm.push(h2 + '.Command.' + k);
    }

    // replace command paths with inputs paths
    for (let onSubmit of onSubmits) {
        var commandFieldToReplace = onSubmit.To.join('.');
        var foundIndex = fieldPathsInForm.indexOf(commandFieldToReplace);

        if (foundIndex >= 0) {
            console.log('commandField to replace : ' + commandFieldToReplace + ' : ' + foundIndex);
            fieldPathsInForm[foundIndex] = onSubmit.From[0];
        }
    }

    //console.log('renderForm2 : ' + h, fieldPathsInForm);

    let html = '';
    for (let f of fieldPathsInForm) {
        console.log('render input : ' + f);
        //console.log(getValue(r.page, f.split('.')));
        html += renderInput(getValue(r.page, f.split('.')), pageName + '.' + f, r);
    }*/


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

function createInput(page, form, hForm, field, hField) {

    if (page['_inputs'] == null)
        page['_inputs'] = [];

    let typeName = null;
    let hCommand = hForm + '.Command';
    if (hField.startsWith(hCommand)) {
        let fieldName = hField.split('.')[hField.split('.').length - 1];
        console.log('CommandFieldTypeNames', form['CommandFieldTypeNames']);
        console.log('fieldName ' + fieldName);
        typeName = form['CommandFieldTypeNames'][fieldName];
    }

    var input = selectInput(field, typeName);
    input.hModel = hField;
    page['_inputs'].push(input);
    return input;
}

function selectInput(x, typeName) {
    if (typeName != null) {
        if (typeName == 'string')
            return new GenericInput('');
        if (typeName == 'double')
            return new GenericInput('number');
        //if (typeName.startsWith('ID<'))
        //    return new GenericInput();
    }
    if (x["IsSelect"])
        return new SelectInput();

    return new GenericInput();
}