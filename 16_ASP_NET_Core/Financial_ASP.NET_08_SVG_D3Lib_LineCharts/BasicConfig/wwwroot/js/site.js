

function drawPieChart(svgSelection, data, options) {
    var width = options.width || 400;
    var height = options.height || 400;
    var isRotateLabel = options.isRotateLabel || true;
    var explodedIndex = options.explodedIndex || 0;
    var explodedLength = options.explodedLength || 0;

    var r = (Math.min(width, height) - 2 * explodedLength) / 2;

    var svg = d3.select(svgSelection)
        .attr("width", width)
        .attr("height", height)
        .append("g")
        .attr("transform", "translate(" + width / 2 + "," + height / 2 + ")");

    var arc = d3.arc()
        .outerRadius(r - 10)
        .innerRadius(0);
    var labelArc = d3.arc()
        .outerRadius(r - 70)
        .innerRadius(r - 70);

    var pie = d3.pie()
        .sort(null)
        .value(function (d) { return d.y; });

    var color = d3.scaleOrdinal(d3.schemeCategory10);
    var g = svg.selectAll(".arc")
        .data(pie(data))
        .enter().append("g")
        .attr("class", "arc");

    var explode = function (x, i) {
        var offset = i === explodedIndex ? explodedLength : 0;
        var angle = (x.startAngle + x.endAngle) / 2;
        var xOff = Math.sin(angle) * offset;
        var yOff = -Math.cos(angle) * offset;
        return "translate(" + xOff + "," + yOff + ")";
    };

    g.append("path")
        .attr("d", arc)
        .style("fill", function (d) { return color(d.data.x); })
        .attr("opacity", 0.7)
        .attr("transform", explode);

    if (!isRotateLabel) {
        g.append("text")
            .attr("transform", function (d) { return "translate(" + labelArc.centroid(d) + ")"; })
            .attr("dy", ".35em")
            .text(function (d) { return d.data.x; });
    } else {
        g.append("text")
            .attr("transform", function (d) {
                var midAngle = d.endAngle < Math.PI ? d.startAngle / 2 + d.endAngle / 2 : d.startAngle / 2 + d.endAngle / 2 + Math.PI;
                return "translate(" + labelArc.centroid(d)[0] + "," + labelArc.centroid(d)[1] + ") rotate(-90) rotate(" + midAngle * 180 / Math.PI + ")";
            })
            .attr("dy", ".35em")
            .attr('text-anchor', 'middle')
            .text(function (d) { return d.data.x; });
    }
}



function drawPolarChart(svgSelection, data, options) {
    var rmax = options.rmax || d3.max(data, function (d) { return d.r; });
    var width = options.width || 400;
    var height = options.height || 300;
    var numCircles = options.numCircles || 5;

    var r = (Math.min(width, height) - 60) / 2;

    var svg = d3.select(svgSelection)
        .attr("width", width)
        .attr("height", height)
        .append("g")
        .attr("transform", "translate(" + width / 2 + "," + height / 2 + ")");

    var rscale = d3.scaleLinear().domain([0, rmax]).range([0, r]);

    var polar = d3.radialLine().radius(function (d) { return rscale(d.r); })
        .angle(function (d) { return -d.angle + Math.PI / 2; });

    var g1 = svg.append("g")
        .attr("class", "r-axis")
        .selectAll("g")
        .data(rscale.ticks(numCircles).slice(1))
        .enter().append("g");
    g1.append("circle")
        .attr("r", rscale)
        .attr("stroke", "black")
        .attr("stroke-width", 1)
        .attr("stroke-dasharray", "1,4")
        .attr("fill", "none");
    g1.append("circle")
        .attr("r", rscale(rmax))
        .attr("stroke", "black")
        .attr("stroke-width", 1)
        .attr("fill", "none");
    g1.append("text")
        .attr("y", function (d) { return -rscale(d) - 4; })
        .attr("transform", "rotate(15)")
        .style("text-anchor", "middle")
        .text(function (d) { return d3.format(".2")(d); });

    var g2 = svg.append("g")
        .attr("class", "angle-axis")
        .selectAll("g")
        .data(d3.range(0, 360, 30))
        .enter().append("g")
        .attr("transform", function (d) { return "rotate(" + -d + ")"; });
    g2.append("line")
        .attr("x2", r)
        .attr("stroke", "black")
        .attr("stroke-width", 1)
        .attr("stroke-dasharray", "1,4");
    g2.append("text")
        .attr("x", r + 6)
        .attr("dy", "0.35em")
        .style("text-anchor", function (d) { return d < 270 && d > 90 ? "end" : null; })
        .attr("transform", function (d) { return d < 270 && d > 90 ? "rotate(180 " + (r + 6) + ",0)" : null; })
        .text(function (d) { return d + "\u00b0"; });

    svg.append("path")
        .attr("class", "line")
        .attr("d", polar(data))
        .attr("stroke", "red")
        .attr("stroke-width", 2)
        .attr("fill", "none");
}

function drawStackedAreaChart(svgSelection, data, options) {
    var ymax = options.ymax || d3.max(data, function (d) { return d.y; });
    var width = options.width || 400;
    var height = options.height || 300;
    var margin = options.margin || { top: 20, right: 20, bottom: 20, left: 20 };
    var isTimeAxis = options.isTimeAxis || false;
    var isGridline = options.isGridline || true;

    options.xdomain = [d3.min(data, function (d) { return d.x; }), d3.max(data, function (d) { return d.x; })];
    options.ydomain = [0, ymax];

    var grid = setAxesGridlines(svgSelection, options);
    var xscale = grid.xscale;
    var yscale = grid.yscale;
    var svg = grid.svg;

    var keys = d3.keys(data[0]).filter(function (key) { return key !== "x"; });
    var color = d3.scaleOrdinal(d3.schemeCategory10).domain(keys);
    var stack = d3.stack().keys(keys);

    var area = d3.area()
        .x(function (d) { return xscale(d.data.x); })
        .y0(function (d) { return yscale(d[0]); })
        .y1(function (d) { return yscale(d[1]); });

    var layer = svg.selectAll(".layer")
        .data(stack(data))
        .enter().append("g")
        .attr("class", "layer");

    layer.append("path")
        .attr("class", "area")
        .style("fill", function (d) { return color(d.key); })
        .attr("opacity", 0.5)
        .attr("d", area);

    var legend = svg.append("g")
        .attr("font-family", "sans-serif")
        .attr("font-size", 11)
        .attr("text-anchor", "end")
        .selectAll("g")
        .data(keys.slice())
        .enter().append("g")
        .attr("transform", function (d, i) { return "translate(0," + i * 22 + ")"; });

    legend.append("rect")
        .attr("x", grid.w - 19)
        .attr("width", 19)
        .attr("height", 19)
        .attr("fill", color)
        .attr("opacity", 0.5);

    legend.append("text")
        .attr("x", grid.w - 24)
        .attr("y", 9.5)
        .attr("dy", "0.32em")
        .text(function (d) { return d; });

    return grid;
}

function drawAreaChart(svgSelection, data, options) {
    var ymax = options.ymax || d3.max(data, function (d) { return d.y; });
    var width = options.width || 400;
    var height = options.height || 300;
    var margin = options.margin || { top: 20, right: 20, bottom: 20, left: 20 };
    var isTimeAxis = options.isTimeAxis || false;
    var isGridline = options.isGridline || true;

    options.xdomain = [d3.min(data, function (d) { return d.x; }), d3.max(data, function (d) { return d.x; })];
    options.ydomain = [0, ymax];

    var grid = setAxesGridlines(svgSelection, options);
    var xscale = grid.xscale;
    var yscale = grid.yscale;
    var svg = grid.svg;

    var area = d3.area()
        .x(function (d) { return xscale(d.x); })
        .y0(yscale(0))
        .y1(function (d) { return yscale(d.y1); });

    svg.append("path")
        .attr("d", area(data))
        .attr("stroke", "black")
        .attr("stroke-width", 1)
        .attr("opacity", 0.5)
        .attr("fill", "red");

    return grid;
}

function drawStairStep(svgSelection, data, stepType, options) {
    var width = options.width || 400;
    var height = options.height || 300;
    var margin = options.margin || { top: 20, right: 20, bottom: 20, left: 20 };
    var isTimeAxis = options.isTimeAxis || false;
    var isGridline = options.isGridline || true;

    var xdomain = [d3.min(data, function (d) { return d.x; }), d3.max(data, function (d) { return d.x; })];
    var ydomain = [d3.min(data, function (d) { return d.y; }), d3.max(data, function (d) { return d.y; })];
    options.xdomain = xdomain;
    options.ydomain = ydomain;

    var grid = setAxesGridlines(svgSelection, options);

    var xscale = grid.xscale;
    var yscale = grid.yscale;
    var svg = grid.svg;

    // add dots and dashed line for original data:
    svg.selectAll("circle")
        .data(data)
        .enter().append("circle")
        .attr("r", 4)
        .attr("cx", function (d) { return xscale(d.x); })
        .attr("cy", function (d) { return yscale(d.y); })
        .attr("fill", "red");

    var line = d3.line()
        .x(function (d) { return xscale(d.x); })
        .y(function (d) { return yscale(d.y); });

    svg.append("path")
        .attr("d", line(data))
        .attr("stroke", "red")
        .attr("stroke-width", 1)
        .attr("stroke-dasharray", "3,3")
        .attr("fill", "none");

    // add step path
    var step;
    if (stepType === "step") {
        step = d3.line().curve(d3.curveStep);
    } else if (stepType === "stepBefore") {
        step = d3.line().curve(d3.curveStepBefore);
    } else if (stepType === "stepAfter") {
        step = d3.line().curve(d3.curveStepAfter);
    }
    step = step.x(function (d) { return xscale(d.x); }).y(function (d) { return yscale(d.y); });
    svg.append("path")
        .attr("stroke-width", 2)
        .attr("stroke", "darkgreen")
        .attr("fill", "none")
        .attr("d", step(data));

    return grid;
}




function drawGroupBarChart(svgSelection, data, options) {
    var width = options.width || 400;
    var height = options.height || 300;
    var margin = options.margin || { top: 20, right: 20, bottom: 20, left: 20 };
    var isGridline = options.isGridline || true;

    var w = width - margin.left - margin.right;
    var h = height - margin.top - margin.bottom;

    var color = d3.scaleOrdinal(d3.schemeCategory10);

    var keys = d3.keys(data[0]).filter(function (key) { return key !== "group"; });
    var xscale = d3.scaleBand().rangeRound([0, w]).paddingInner(0.1).domain(data.map(function (d) { return d.group; }));
    var x1scale = d3.scaleBand().padding(0.1).domain(keys).rangeRound([0, xscale.bandwidth()]);
    var ymax = d3.max(data, function (d) { return d3.max(keys, function (key) { return d[key]; }); });
    var yscale = d3.scaleLinear().rangeRound([h, 0]).domain([0, ymax]);

    var svg = setBarAxesGridlines(svgSelection, xscale, yscale, width, height, margin, w, h, isGridline);

    svg.append("g")
        .selectAll("g")
        .data(data)
        .enter().append("g")
        .attr("transform", function (d) { return "translate(" + xscale(d.group) + ",0)"; })
        .selectAll("rect")
        .data(function (d) { return keys.map(function (key) { return { key: key, value: d[key] }; }); })
        .enter().append("rect")
        .attr("x", function (d) { return x1scale(d.key); })
        .attr("y", function (d) { return yscale(d.value); })
        .attr("width", x1scale.bandwidth())
        .attr("height", function (d) { return h - yscale(d.value); })
        .attr("fill", function (d) { return color(d.key); });

    var legend = svg.append("g")
        .attr("font-family", "sans-serif")
        .attr("font-size", 12)
        .attr("text-anchor", "end")
        .selectAll("g")
        .data(keys.slice())
        .enter().append("g")
        .attr("transform", function (d, i) { return "translate(0," + i * 22 + ")"; });

    legend.append("rect")
        .attr("x", w + 5)
        .attr("width", 20)
        .attr("height", 20)
        .attr("fill", color);

    legend.append("text")
        .attr("x", w + 35)
        .attr("y", 9.5)
        .attr("dy", "0.32em")
        .text(function (d) { return d; });

    return svg;
}


function drawBarChart(svgSelection, data, options) {
    var width = options.width || 400;
    var height = options.height || 300;
    var margin = options.margin || { top: 20, right: 20, bottom: 20, left: 20 };
    var isGridline = options.isGridline || true;

    var w = width - margin.left - margin.right;
    var h = height - margin.top - margin.bottom;
    var xscale = d3.scaleBand().domain(data.map(function (d) { return d.x; })).range([0, w]).padding(0.2);
    var ymax = d3.max(data, function (d) { return d.y; });
    var yscale = d3.scaleLinear().domain([0, ymax]).range([h, 0]);

    var svg = setBarAxesGridlines(svgSelection, xscale, yscale, width, height, margin, w, h, isGridline);

    svg.selectAll("bar")
        .data(data)
        .enter()
        .append("rect")
        .attr("fill", "steelblue")
        .attr("x", function (d) { return xscale(d.x); })
        .attr("y", function (d) { return yscale(d.y); })
        .attr("width", xscale.bandwidth())
        .attr("height", function (d) { return h - yscale(d.y); });

    return svg;
}


function setBarAxesGridlines(svgSelection, xscale, yscale, width, height, margin, w, h, isGridline) {

    var svg = d3.select(svgSelection)
        .attr("width", width)
        .attr("height", height)
        .append("g")
        .attr("transform", "translate(" + margin.left + "," + margin.top + ")");

    // box for chart area
    svg.append("rect")
        .attr("width", w)
        .attr("height", h)
        .style("fill", "none")
        .style("stroke", "black")
        .style("stroke-width", 0.2);

    // x-axis
    var xaxis = d3.axisBottom().scale(xscale);
    svg.append("g")
        .attr("class", "x-axis")
        .attr("transform", "translate(0," + h + ")")
        .call(xaxis);

    // y-axis
    var yaxis = d3.axisLeft().scale(yscale);
    svg.append("g")
        .attr("class", "y-axis")
        .call(yaxis);

    // y grid lines:
    if (isGridline) {
        svg.selectAll("line.ygrid")
            .data(yscale.ticks())
            .enter()
            .append("line")
            .attr("class", "ygrid")
            .attr("x1", 0)
            .attr("x2", w)
            .attr("y1", function (d) { return yscale(d); })
            .attr("y2", function (d) { return yscale(d); })
            .attr("stroke", "lightgray")
            .attr("stroke-width", "1px");
    }
    return svg;
}




function drawY2LineChart(svgSelection, data, options) {
    var width = options.width || 400;
    var height = options.height || 300;
    var margin = options.margin || { top: 20, right: 20, bottom: 20, left: 20 };
    var isTimeAxis = options.isTimeAxis || false;
    var isGridline = options.isGridline || true;

    var xdomain = [d3.min(data, function (d) { return d.x; }), d3.max(data, function (d) { return d.x; })];
    var ydomain = [d3.min(data, function (d) { return d.y; }), d3.max(data, function (d) { return d.y; })];
    var y2domain = [d3.min(data, function (d) { return d.y2; }), d3.max(data, function (d) { return d.y2; })];
    options.xdomain = xdomain;
    options.ydomain = ydomain;

    var grid = setAxesGridlines(svgSelection, options);
    var xscale = grid.xscale;
    var yscale = grid.yscale;
    var svg = grid.svg;
    var grid2 = addY2Gridlines(svg, y2domain, grid.w, grid.h, margin, isGridline);
    var y2scale = grid2.y2scale;

    var gen = d3.line()
        .x(function (d) { return xscale(d.x); })
        .y(function (d) { return yscale(d.y); });

    var gen2 = d3.line()
        .x(function (d) { return xscale(d.x); })
        .y(function (d) { return y2scale(d.y2); });

    svg.append("path")
        .attr("d", gen(data))
        .attr("stroke", "black")
        .attr("stroke-width", 2)
        .attr("fill", "none");

    svg.append("path")
        .attr("d", gen2(data))
        .attr("stroke", "green")
        .attr("stroke-width", 2)
        .attr("fill", "none");

    return grid;
}


function addY2Gridlines(svg, y2domain, w, h, margin, isGridline) {
    isGridline = isGridline || true;

    y2scale = d3.scaleLinear().domain(y2domain).range([h, 0]);
    var yaxis = d3.axisRight().scale(y2scale);
    svg.append("g")
        .attr("transform", "translate(" + w + ",0)")
        .attr("class", "y-axis")
        .call(yaxis);

    if (isGridline) {
        svg.selectAll("line.y2grid")
            .data(y2scale.ticks())
            .enter()
            .append("line")
            .attr("class", "y2grid")
            .attr("x1", 0)
            .attr("x2", w)
            .attr("y1", function (d) { return y2scale(d); })
            .attr("y2", function (d) { return y2scale(d); })
            .attr("stroke", "lightgreen")
            .attr("stroke-width", "1px");
    }
    return { y2scale };
}

function addY2Label(svg, w, h, options) {
    var margin = options.margin || { top: 20, right: 20, bottom: 20, left: 20 };
    var y2label = options.ylabel || "Y2 Axis";

    // y2 label
    svg.append("text")
        .attr("text-anchor", "middle")
        .attr("transform", "translate(" + (w + margin.right - 15) + "," + (h / 2) + ") rotate(90)")
        .style("font-size", "12px")
        .text(y2label);
}

function drawMultiLineChart(svgSelection, data, options) {
    var width = options.width || 400;
    var height = options.height || 300;
    var margin = options.margin || { top: 20, right: 20, bottom: 20, left: 20 };
    var isTimeAxis = options.isTimeAxis || false;
    var isGridline = options.isGridline || true;
    var isLegend = options.isLegend || true;

    var xdomain = [d3.min(data, function (d) { return d.x; }), d3.max(data, function (d) { return d.x; })];
    var ydomain = [d3.min(data, function (d) { return d.y; }), d3.max(data, function (d) { return d.y; })];
    options.xdomain = xdomain;
    options.ydomain = ydomain;

    var grid = setAxesGridlines(svgSelection, options);

    var xscale = grid.xscale;
    var yscale = grid.yscale;
    var svg = grid.svg;
    var color = d3.scaleOrdinal(d3.schemeCategory10);

    var gen = d3.line()
        .curve(d3.curveBasis)
        .x(function (d) { return xscale(d.x); })
        .y(function (d) { return yscale(d.y); });

    var dataGroup = d3.nest().key(function (d) { return d.id; }).entries(data);

    dataGroup.forEach(function (d, i) {
        svg.append("path")
            .attr("d", gen(d.values))
            .attr("stroke", function () { return d.color = color(d.key); })
            .attr("stroke-width", 2)
            .attr("fill", "none");

        // Add legend:
        if (isLegend) {
            var lgdspace = width / (dataGroup.length + 1);
            svg.append("text")
                .attr("x", 0.5 * lgdspace + i * lgdspace)
                .attr("y", -10)
                .attr("fill", function () { return d.color = color(d.key); })
                .text(d.key);
        }
    });

    return grid;
}


function drawLineChart(svgSelection, data, options) {
    var width = options.width || 400;
    var height = options.height || 300;
    var margin = options.margin || { top: 20, right: 20, bottom: 20, left: 20 };
    var isTimeAxis = options.isTimeAxis || false;
    var isGridline = options.isGridline || true;

    var xdomain = [d3.min(data, function (d) { return d.x; }), d3.max(data, function (d) { return d.x; })];
    var ydomain = [d3.min(data, function (d) { return d.y; }), d3.max(data, function (d) { return d.y; })];

    options.xdomain = xdomain;
    options.ydomain = ydomain;

    var grid = setAxesGridlines(svgSelection, options);
    var xscale = grid.xscale;
    var yscale = grid.yscale;
    var svg = grid.svg;

    var gen = d3.line()
        .x(function (d) { return xscale(d.x); })
        .y(function (d) { return yscale(d.y); });

    svg.append("path")
        .attr("d", gen(data))
        .attr("stroke", "black")
        .attr("stroke-width", 2)
        .attr("fill", "none");

    //console.log(svgSelection);
    //console.log(data);
    //console.log(options);

    return grid;
}


function addTitleLabel(svg, w, h, options) {
    var title = options.title || "Title";
    var xlabel = options.xlable || "X Axis";
    var ylabel = options.ylabel || "Y Axis";
    var margin = options.margin || { top: 20, right: 20, bottom: 20, left: 20 };

    // title
    svg.append("text")
        .attr("x", (w / 2))
        .attr("y", -margin.top + 15)
        .attr("text-anchor", "middle")
        .style("font-size", "14px")
        .text(title);

    // x label
    svg.append("text")
        .attr("x", w / 2)
        .attr("y", h + margin.bottom - 15)
        .attr("text-anchor", "middle")
        .style("font-size", "12px")
        .text(xlabel);

    // y label
    svg.append("text")
        .attr("text-anchor", "middle")
        .attr("transform", "translate(" + (-margin.left + 15) + "," + (h / 2) + ") rotate(-90)")
        .style("font-size", "12px")
        .text(ylabel);
}



function setAxesGridlines(svgSelection, options) {

    // define default values
    var xdomain = options.xdomain || [0, 100];
    var ydomain = options.ydomain || [0, 100];
    var width = options.width || 400;
    var height = options.height || 300;
    var margin = options.margin || { top: 20, right: 20, bottom: 20, left: 20 };
    var isTimeAxis = options.isTimeAxis || false;
    var isGridline = options.isGridline || true;

    var w = width - margin.left - margin.right;
    var h = height - margin.top - margin.bottom;

    var xscale, yscale;
    var svg = d3.select(svgSelection)
        .attr("width", width)
        .attr("height", height)
        .append("g")
        .attr("transform", "translate(" + margin.left + "," + margin.top + ")");

    // box for chart area
    svg.append("rect")
        .attr("width", w)
        .attr("height", h)
        .style("fill", "none")
        .style("stroke", "black")
        .style("stroke-width", 0.2);

    //X-axis:
    if (!isTimeAxis) {
        xscale = d3.scaleLinear().domain(xdomain).range([0, w]);
        var xaxis = d3.axisBottom().scale(xscale);
        svg.append("g")
            .attr("class", "x-axis")
            .attr("transform", "translate(0," + h + ")")
            .call(xaxis);
    } else {
        xscale = d3.scaleTime().domain(xdomain).range([0, w]);
        var xaxis1 = d3.axisBottom().scale(xscale);
        svg.append("g")
            .attr("class", "x-axis")
            .attr("transform", "translate(0," + h + ")")
            .call(xaxis1)
            .selectAll("text")
            .style("text-anchor", "end")
            .attr("dx", "-0.8em")
            .attr("dy", "0.15em")
            .attr("transform", "rotate(-60)");
    }


    //Y-axis:
    yscale = d3.scaleLinear().domain(ydomain).range([h, 0]);
    var yaxis = d3.axisLeft().scale(yscale);
    svg.append("g")
        .attr("class", "y-axis")
        .call(yaxis);


    //grid lines:
    if (isGridline) {
        svg.selectAll("line.ygrid")
            .data(yscale.ticks())
            .enter()
            .append("line")
            .attr("class", "ygrid")
            .attr("x1", 0)
            .attr("x2", w)
            .attr("y1", function (d) { return yscale(d); })
            .attr("y2", function (d) { return yscale(d); })
            .attr("stroke", "lightgray")
            .attr("stroke-width", "1px");

        svg.selectAll("line.xgrid")
            .data(xscale.ticks())
            .enter()
            .append("line")
            .attr("class", "xgrid")
            .attr("x1", function (d) { return xscale(d); })
            .attr("x2", function (d) { return xscale(d); })
            .attr("y1", 0)
            .attr("y2", h)
            .attr("stroke", "lightgray")
            .attr("stroke-width", "1px");
    }
    return { xscale, yscale, svg, w, h };
}



