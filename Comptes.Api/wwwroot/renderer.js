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
        let found = null;
        for (let r of _renderers.renderers) {
            if (r.predicate(x, h, this))
                found = r;
        }
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
}
var _renderers = new RenderConf();


