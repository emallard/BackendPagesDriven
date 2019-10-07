
class GenericInput {
    constructor(inputType) {
        this.inputType = inputType;
        this.hModel = '';
        this.id = '';
        this.page = null;
    }
    render(x, id, r) {
        let f = field(id);
        this.id = id;
        this.page = r.page;

        //r.afterRender(() => {
        //    document.getElementById(id).addEventListener('change', () => this.updateModel());
        //});

        return `<div class="form-group">
                    <label for="${id}">${f}</label>
                    <input class="form-control" type="${this.inputType}" id="${id}"></input>
                </div>`;
    }

    updateFromModel() {
        if (!valueExists(this.page, this.hModel.split('.')))
            return;
        //console.log('updateFromModel ' + this.id + ' : ' + this.hModel);
        let value = getValue(this.page, this.hModel.split('.'));
        //console.log(this.page, value);
        if (this.inputType == 'date')
            value = value.substring(0, value.indexOf('T'));
        document.getElementById(this.id).value = value;
    }

    updateModel() {
        let value = document.getElementById(this.id).value;
        setValue(this.page, this.hModel.split('.'), value);
    }
}

class SelectInput {
    constructor() {
        this.hModel = '';
        this.id = '';
        this.page = null;
    }

    render(x, id, r) {
        let f = field(id);
        this.page = r.page;
        this.id = id;

        r.afterRender(async () => {

            //document.getElementById(id).addEventListener('change', () => this.updateModel());

            let response = await call(x.Source);
            x.Source.Result = response;
            this.updateFromModel();
        });
        return `<div class="form-group">
            <label for="${id}">${f}</label>
            <select class="form-control" id="${id}">
            </select>
        </div>`;
    }

    updateFromModel() {
        let select = getValue(this.page, this.hModel.split('.'));
        let selectElt = document.getElementById(this.id);
        let sourceResult = select['Source']['Result'];
        if (sourceResult != null)
            selectElt.innerHTML = sourceResult.map(x => `<option value="${x.Value}">${x.Label}</option>`).join('');

        let selected = select['Selected'];
        if (selected != null) {
            for (let i = 0; i < selectElt.options.length; i++) {
                let option = selectElt.options[i];
                if (option.value == selected.Value) {
                    selectElt.selectedIndex = i;
                }
            }
        }
    }

    updateModel() {
        let selectElt = document.getElementById(this.id);
        //let selectedIndex = selectElt.selectedIndex;
        let selectedValue = selectElt.selectedOptions[0].value;

        let selectModel = getValue(this.page, this.hModel.split('.'));
        selectModel.Selected = { Value: selectedValue };
    }
}