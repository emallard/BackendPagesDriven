let guard = 0;
class Renderer {
    constructor(p) {
        this.page = p;
        this.afterRenders = [];
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
            applyOnInits(this.page)
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


