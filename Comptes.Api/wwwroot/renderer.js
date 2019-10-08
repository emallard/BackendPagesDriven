let guard = 0;
class Renderer {

    constructor(p) {
        this.page = p;
        this.afterRenders = [];
        this.inputs = [];
    }

    afterRender(callback) {
        this.afterRenders.push(callback);
    }

    callAfterRender() {
        console.log(`call after render (${this.afterRenders.length})`);
        try {
            for (let f of this.afterRenders) {
                f();
            }
        }
        finally {
            this.afterRenders = [];
        }


    }

    renderTo(x, id, elt) {
        elt.innerHTML = this.render(x, id);
        setTimeout(() => {
            this.callAfterRender();
            this.applyOnInits()
        }, 0);
    }

    render(x, id) {
        console.log("render " + id);
        guard++;
        if (guard > 1000) {
            console.log("GUARD TOO MANY CALLS : " + guard);
            return "";
        }

        if (x == null)
            return '';
        let found = _renderers.find(x, id, this);
        if (found == null)
            console.error('not found renderer : ' + id, x);
        return found.func(x, id, this);
    }

    applyOnInits() {
        console.log('applyOnInits');
        for (let init of this.page.OnInits) {
            if (valueExists(this.page, init.From)) {
                let v = getValue(this.page, init.From);

                console.log('applyOnInits setValue ' + init.To.join('.'), v);
                setValue(this.page, init.To, v);
                this.onPageUpdate(init.To.join('.'));
                /*
                // update Inputs from models
                for (var input of _pageinputs) {
                    input.updateFromModel();
                }
                */
            }
        }
    }

    applyOnSubmits() {
        console.log('applyOnSubmits');
        for (let s of this.page.OnSubmits) {
            console.log('  ' + s.From.join('.') + ' => ' + s.To.join('.'));
            let v = getValue(this.page, s.From);
            console.log('  ' + v);
            setValue(this.page, s.To, v);
        }
    }

    applyInputsToModel() {
        for (let input of this.inputs)
            input.updateModel();
    }

    onPageUpdate(id) {
        console.log('onPageUpdate ' + id);
        for (let input of this.inputs)
            input.onPageUpdate(id);
    }
}


class RenderConf {

    constructor() {
        this.renderers = [];
    }
    push(predicate, func) {
        this.renderers.push({ predicate: predicate, func: func })
    }
    find(x, id, r) {
        let found = null;
        for (let elt of this.renderers) {
            if (elt.predicate(x, id, r))
                found = elt;
        }
        return found;
    }

    findSpecific(x, id, r) {
        for (let i = 3; i < this.renderers.length; ++i) {
            let elt = this.renderers[i];
            if (elt.predicate(x, id, r))
                return true;
        }
        return false;
    }
}
var _renderers = new RenderConf();


