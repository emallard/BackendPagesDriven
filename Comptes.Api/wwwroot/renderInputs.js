
class GenericInput {
    constructor(inputType) {
        this.inputType = inputType;
        this.hModel = '';
        this.id = '';
        this.page = null;
    }
    render(x, h, r) {
        let f = field(h);
        this.id = h;
        this.page = r.page;

        return `<div class="form-group">
                    <label for="${h}">${f}</label>
                    <input class="form-control" type="${this.inputType}" id="${h}"></input>
                </div>`;
    }

    updateFromModel() {
        //console.log('updateFromModel ' + this.id + ' : ' + this.hModel);
        let value = getValue(this.page, this.hModel.split('.'));
        //console.log(this.page, value);
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

    render(x, h, r) {
        let f = field(h);
        this.page = r.page;
        this.id = h;

        r.afterRender(async () => {
            let response = await call(x.Source);
            x.Source.Result = response;
            this.updateFromModel();
        });
        return `<div class="form-group">
            <label for="${h}">${f}</label>
            <select class="form-control" id="${h}">
            </select>
        </div>`;
    }

    updateFromModel() {
        let select = getValue(this.page, this.hModel.split('.'));
        console.log('SELECT MODEL ', select);
        let selectElt = document.getElementById(this.id);
        let selected = select['Selected'];
        let sourceResult = select['Source']['Result'];
        if (sourceResult != null)
            selectElt.innerHTML = sourceResult.map(x => `<option value="${x.Value}">${x.Label}</option>`).join('');
    }

    updateModel() {
        let selectElt = document.getElementById(this.id);
        //let selectedIndex = selectElt.selectedIndex;
        let selectedValue = selectElt.selectedOptions[0].value;

        let selectModel = getValue(this.page, this.hModel.split('.'));
        selectModel.Selected = { Value: selectedValue };
    }
}