
_renderers.push(
    (x, h, r) => h == 'TestPageResponse.PageGraph.Result',
    (x, h, r) => {
        r.afterRender(() => setTimeout(() =>
            drawInGraph(
                _page.TestReport.Result.Path.Items,
                document.getElementById('TestPageResponse.PageGraph.Result.Svg'))
            , 1000));
        return `${x.Svg}`.replace('<svg', `<svg id="${h + '.Svg'}"`);
    }
);

