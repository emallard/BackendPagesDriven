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

    renderTo(x, h, elt) {
        elt.innerHTML = this.render(x, h);
        setTimeout(() => {
            this.callAfterRender();
            applyOnInits(this.page)
        }, 0);
    }

    render(x, h) {
        console.log("render " + h);
        guard++;
        if (guard > 1000) {
            console.log("GUARD TOO MANY CALLS : " + guard);
            return "";
        }

        if (x == null)
            return '';
        let found = _renderers.find(x, h, this);
        if (found == null)
            console.error('not found renderer : ' + h, x);
        return found.func(x, h, this);
    }
}


class RenderConf {

    constructor() {
        this.renderers = [];
    }
    push(predicate, func) {
        this.renderers.push({ predicate: predicate, func: func })
    }
    find(x, h, r) {
        let found = null;
        for (let elt of this.renderers) {
            if (elt.predicate(x, h, r))
                found = elt;
        }
        return found;
    }

    findSpecific(x, h, r) {
        for (let i = 3; i < this.renderers.length; ++i) {
            let elt = this.renderers[i];
            if (elt.predicate(x, h, r))
                return true;
        }
        return false;
    }
}
var _renderers = new RenderConf();


