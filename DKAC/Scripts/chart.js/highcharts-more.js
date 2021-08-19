﻿/*
 Highcharts JS v8.0.4 (2020-03-10)

 (c) 2009-2018 Torstein Honsi

 License: www.highcharts.com/license
*/
(function (f) { "object" === typeof module && module.exports ? (f["default"] = f, module.exports = f) : "function" === typeof define && define.amd ? define("highcharts/highcharts-more", ["highcharts"], function (E) { f(E); f.Highcharts = E; return f }) : f("undefined" !== typeof Highcharts ? Highcharts : void 0) })(function (f) {
    function E(l, a, c, b) { l.hasOwnProperty(a) || (l[a] = b.apply(null, c)) } f = f ? f._modules : {}; E(f, "parts-more/Pane.js", [f["parts/Globals.js"], f["parts/Utilities.js"]], function (l, a) {
        function c(d, b, n) {
            return Math.sqrt(Math.pow(d -
                n[0], 2) + Math.pow(b - n[1], 2)) < n[2] / 2
        } var b = a.addEvent, u = a.extend, v = a.merge, w = a.pick, f = a.splat, y = l.CenteredSeriesMixin; l.Chart.prototype.collectionsWithUpdate.push("pane"); a = function () {
            function d(d, b) {
            this.options = this.chart = this.center = this.background = void 0; this.coll = "pane"; this.defaultOptions = { center: ["50%", "50%"], size: "85%", innerSize: "0%", startAngle: 0 }; this.defaultBackgroundOptions = {
                shape: "circle", borderWidth: 1, borderColor: "#cccccc", backgroundColor: {
                    linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 }, stops: [[0,
                        "#ffffff"], [1, "#e6e6e6"]]
                }, from: -Number.MAX_VALUE, innerRadius: 0, to: Number.MAX_VALUE, outerRadius: "105%"
            }; this.init(d, b)
            } d.prototype.init = function (d, b) { this.chart = b; this.background = []; b.pane.push(this); this.setOptions(d) }; d.prototype.setOptions = function (d) { this.options = v(this.defaultOptions, this.chart.angular ? { background: {} } : void 0, d) }; d.prototype.render = function () {
                var d = this.options, b = this.options.background, a = this.chart.renderer; this.group || (this.group = a.g("pane-group").attr({ zIndex: d.zIndex || 0 }).add());
                this.updateCenter(); if (b) for (b = f(b), d = Math.max(b.length, this.background.length || 0), a = 0; a < d; a++)b[a] && this.axis ? this.renderBackground(v(this.defaultBackgroundOptions, b[a]), a) : this.background[a] && (this.background[a] = this.background[a].destroy(), this.background.splice(a, 1))
            }; d.prototype.renderBackground = function (d, b) {
                var a = "animate", n = { "class": "highcharts-pane " + (d.className || "") }; this.chart.styledMode || u(n, { fill: d.backgroundColor, stroke: d.borderColor, "stroke-width": d.borderWidth }); this.background[b] ||
                    (this.background[b] = this.chart.renderer.path().add(this.group), a = "attr"); this.background[b][a]({ d: this.axis.getPlotBandPath(d.from, d.to, d) }).attr(n)
            }; d.prototype.updateCenter = function (d) { this.center = (d || this.axis || {}).center = y.getCenter.call(this) }; d.prototype.update = function (d, b) { v(!0, this.options, d); v(!0, this.chart.options.pane, d); this.setOptions(this.options); this.render(); this.chart.axes.forEach(function (d) { d.pane === this && (d.pane = null, d.update({}, b)) }, this) }; return d
        }(); l.Chart.prototype.getHoverPane =
            function (d) { var b = this, a; d && b.pane.forEach(function (n) { var m = d.chartX - b.plotLeft, t = d.chartY - b.plotTop; c(b.inverted ? t : m, b.inverted ? m : t, n.center) && (a = n) }); return a }; b(l.Chart, "afterIsInsidePlot", function (d) { this.polar && (d.isInsidePlot = this.pane.some(function (b) { return c(d.x, d.y, b.center) })) }); b(l.Pointer, "beforeGetHoverData", function (d) {
                var b = this.chart; b.polar && (b.hoverPane = b.getHoverPane(d), d.filter = function (a) {
                    return a.visible && !(!d.shared && a.directTouch) && w(a.options.enableMouseTracking, !0) && (!b.hoverPane ||
                        a.xAxis.pane === b.hoverPane)
                })
            }); b(l.Pointer, "afterGetHoverData", function (d) { var b = this.chart; d.hoverPoint && d.hoverPoint.plotX && d.hoverPoint.plotY && b.hoverPane && !c(d.hoverPoint.plotX, d.hoverPoint.plotY, b.hoverPane.center) && (d.hoverPoint = void 0) }); l.Pane = a; return l.Pane
    }); E(f, "parts-more/RadialAxis.js", [f["parts/Globals.js"], f["parts/Tick.js"], f["parts/Utilities.js"]], function (l, a, c) {
        var b = c.addEvent, u = c.correctFloat, v = c.defined, w = c.extend, f = c.merge, y = c.pick, d = c.pInt, m = c.relativeLength; c = c.wrap; var n =
            l.Axis, t = l.noop, x = n.prototype, A = a.prototype; var r = { getOffset: t, redraw: function () { this.isDirty = !1 }, render: function () { this.isDirty = !1 }, createLabelCollector: function () { return !1 }, setScale: t, setCategories: t, setTitle: t }; var p = {
                defaultRadialGaugeOptions: { labels: { align: "center", x: 0, y: null }, minorGridLineWidth: 0, minorTickInterval: "auto", minorTickLength: 10, minorTickPosition: "inside", minorTickWidth: 1, tickLength: 10, tickPosition: "inside", tickWidth: 2, title: { rotation: 0 }, zIndex: 2 }, defaultCircularOptions: {
                    gridLineWidth: 1,
                    labels: { align: null, distance: 15, x: 0, y: null, style: { textOverflow: "none" } }, maxPadding: 0, minPadding: 0, showLastLabel: !1, tickLength: 0
                }, defaultRadialOptions: { gridLineInterpolation: "circle", gridLineWidth: 1, labels: { align: "right", x: -3, y: -2 }, showLastLabel: !1, title: { x: 4, text: null, rotation: 90 } }, setOptions: function (h) { h = this.options = f(this.defaultOptions, this.defaultPolarOptions, h); h.plotBands || (h.plotBands = []); l.fireEvent(this, "afterSetOptions") }, getOffset: function () {
                    x.getOffset.call(this); this.chart.axisOffset[this.side] =
                        0
                }, getLinePath: function (h, g, e) {
                    h = this.pane.center; var k = this.chart, p = y(g, h[2] / 2 - this.offset); "undefined" === typeof e && (e = this.horiz ? 0 : this.center && -this.center[3] / 2); e && (p += e); this.isCircular || "undefined" !== typeof g ? (g = this.chart.renderer.symbols.arc(this.left + h[0], this.top + h[1], p, p, { start: this.startAngleRad, end: this.endAngleRad, open: !0, innerR: 0 }), g.xBounds = [this.left + h[0]], g.yBounds = [this.top + h[1] - p]) : (g = this.postTranslate(this.angleRad, p), g = ["M", this.center[0] + k.plotLeft, this.center[1] + k.plotTop,
                        "L", g.x, g.y]); return g
                }, setAxisTranslation: function () { x.setAxisTranslation.call(this); this.center && (this.transA = this.isCircular ? (this.endAngleRad - this.startAngleRad) / (this.max - this.min || 1) : (this.center[2] - this.center[3]) / 2 / (this.max - this.min || 1), this.minPixelPadding = this.isXAxis ? this.transA * this.minPointOffset : 0) }, beforeSetTickPositions: function () {
                this.autoConnect = this.isCircular && "undefined" === typeof y(this.userMax, this.options.max) && u(this.endAngleRad - this.startAngleRad) === u(2 * Math.PI); !this.isCircular &&
                    this.chart.inverted && this.max++; this.autoConnect && (this.max += this.categories && 1 || this.pointRange || this.closestPointRange || 0)
                }, setAxisSize: function () { x.setAxisSize.call(this); if (this.isRadial) { this.pane.updateCenter(this); var h = this.center = w([], this.pane.center); if (this.isCircular) this.sector = this.endAngleRad - this.startAngleRad; else { var g = this.postTranslate(this.angleRad, h[3] / 2); h[0] = g.x - this.chart.plotLeft; h[1] = g.y - this.chart.plotTop } this.len = this.width = this.height = (h[2] - h[3]) * y(this.sector, 1) / 2 } },
                getPosition: function (h, g) { h = this.translate(h); return this.postTranslate(this.isCircular ? h : this.angleRad, y(this.isCircular ? g : 0 > h ? 0 : h, this.center[2] / 2) - this.offset) }, postTranslate: function (h, g) { var e = this.chart, k = this.center; h = this.startAngleRad + h; return { x: e.plotLeft + k[0] + Math.cos(h) * g, y: e.plotTop + k[1] + Math.sin(h) * g } }, getPlotBandPath: function (h, g, e) {
                    var k = this.center, p = this.startAngleRad, C = k[2] / 2, q = [y(e.outerRadius, "100%"), e.innerRadius, y(e.thickness, 10)], r = Math.min(this.offset, 0), b = /%$/; var a = this.isCircular;
                    if ("polygon" === this.options.gridLineInterpolation) q = this.getPlotLinePath({ value: h }).concat(this.getPlotLinePath({ value: g, reverse: !0 })); else {
                        h = Math.max(h, this.min); g = Math.min(g, this.max); a || (q[0] = this.translate(h), q[1] = this.translate(g)); q = q.map(function (e) { b.test(e) && (e = d(e, 10) * C / 100); return e }); if ("circle" !== e.shape && a) h = p + this.translate(h), g = p + this.translate(g); else { h = -Math.PI / 2; g = 1.5 * Math.PI; var n = !0 } q[0] -= r; q[2] -= r; q = this.chart.renderer.symbols.arc(this.left + k[0], this.top + k[1], q[0], q[0], {
                            start: Math.min(h,
                                g), end: Math.max(h, g), innerR: y(q[1], q[0] - q[2]), open: n
                        }); a && (a = (g + h) / 2, r = this.left + k[0] + k[2] / 2 * Math.cos(a), q.xBounds = a > -Math.PI / 2 && a < Math.PI / 2 ? [r, this.chart.plotWidth] : [0, r], q.yBounds = [this.top + k[1] + k[2] / 2 * Math.sin(a)], q.yBounds[0] += a > -Math.PI && 0 > a || a > Math.PI ? -10 : 10)
                    } return q
                }, getCrosshairPosition: function (h, g, e) {
                    var k = h.value, p = this.pane.center; if (this.isCircular) {
                        if (v(k)) h.point && (d = h.point.shapeArgs || {}, d.start && (k = this.chart.inverted ? this.translate(h.point.rectPlotY, !0) : h.point.x)); else {
                            var d = h.chartX ||
                                0; var q = h.chartY || 0; k = this.translate(Math.atan2(q - e, d - g) - this.startAngleRad, !0)
                        } h = this.getPosition(k); d = h.x; q = h.y
                    } else v(k) || (d = h.chartX, q = h.chartY), v(d) && v(q) && (e = p[1] + this.chart.plotTop, k = this.translate(Math.min(Math.sqrt(Math.pow(d - g, 2) + Math.pow(q - e, 2)), p[2] / 2) - p[3] / 2, !0)); return [k, d || 0, q || 0]
                }, getPlotLinePath: function (h) {
                    var g = this, e = g.pane.center, k = g.chart, p = k.inverted, d = h.value, q = h.reverse, r = g.getPosition(d), b = g.pane.options.background ? g.pane.options.background[0] || g.pane.options.background :
                        {}, a = b.innerRadius || "0%", n = b.outerRadius || "100%"; b = e[0] + k.plotLeft; var c = e[1] + k.plotTop, t = r.x, x = r.y, u = g.height; r = e[3] / 2; var v, l; h.isCrosshair && (x = this.getCrosshairPosition(h, b, c), d = x[0], t = x[1], x = x[2]); if (g.isCircular) { q = Math.sqrt(Math.pow(t - b, 2) + Math.pow(x - c, 2)); a = "string" === typeof a ? m(a, 1) : a / q; n = "string" === typeof n ? m(n, 1) : n / q; e && r && (e = r / q, a < e && (a = e), n < e && (n = e)); var w = ["M", b + a * (t - b), c - a * (c - x), "L", t - (1 - n) * (t - b), x + (1 - n) * (c - x)] } else (d = g.translate(d)) && (0 > d || d > u) && (d = 0), "circle" === g.options.gridLineInterpolation ?
                            w = g.getLinePath(0, d, r) : (k[p ? "yAxis" : "xAxis"].forEach(function (e) { e.pane === g.pane && (v = e) }), w = [], e = v.tickPositions, v.autoConnect && (e = e.concat([e[0]])), q && (e = [].concat(e).reverse()), d && (d += r), e.forEach(function (e, k) { l = v.getPosition(e, d); w.push(k ? "L" : "M", l.x, l.y) })); return w
                }, getTitlePosition: function () { var h = this.center, g = this.chart, e = this.options.title; return { x: g.plotLeft + h[0] + (e.x || 0), y: g.plotTop + h[1] - { high: .5, middle: .25, low: 0 }[e.align] * h[2] + (e.y || 0) } }, createLabelCollector: function () {
                    var h = this; return function () {
                        if (h.isRadial &&
                            h.tickPositions && !0 !== h.options.labels.allowOverlap) return h.tickPositions.map(function (g) { return h.ticks[g] && h.ticks[g].label }).filter(function (g) { return !!g })
                    }
                }
            }; b(n, "init", function (h) {
                var g = this.chart, e = g.inverted, k = g.angular, d = g.polar, b = this.isXAxis, q = this.coll, a = k && b, n, c = g.options; h = h.userOptions.pane || 0; h = this.pane = g.pane && g.pane[h]; if ("colorAxis" === q) this.isRadial = !1; else {
                    if (k) { if (w(this, a ? r : p), n = !b) this.defaultPolarOptions = this.defaultRadialGaugeOptions } else d && (w(this, p), this.defaultPolarOptions =
                        (n = this.horiz) ? this.defaultCircularOptions : f("xAxis" === q ? this.defaultOptions : this.defaultYAxisOptions, this.defaultRadialOptions), e && "yAxis" === q && (this.defaultPolarOptions.stackLabels = this.defaultYAxisOptions.stackLabels)); k || d ? (this.isRadial = !0, c.chart.zoomType = null, this.labelCollector || (this.labelCollector = this.createLabelCollector()), this.labelCollector && g.labelCollectors.push(this.labelCollector)) : this.isRadial = !1; h && n && (h.axis = this); this.isCircular = n
                }
            }); b(n, "afterInit", function () {
                var h = this.chart,
                g = this.options, e = this.pane, k = e && e.options; h.angular && this.isXAxis || !e || !h.angular && !h.polar || (this.angleRad = (g.angle || 0) * Math.PI / 180, this.startAngleRad = (k.startAngle - 90) * Math.PI / 180, this.endAngleRad = (y(k.endAngle, k.startAngle + 360) - 90) * Math.PI / 180, this.offset = g.offset || 0)
            }); b(n, "autoLabelAlign", function (h) { this.isRadial && (h.align = void 0, h.preventDefault()) }); b(n, "destroy", function () {
                if (this.chart && this.chart.labelCollectors) {
                    var h = this.chart.labelCollectors.indexOf(this.labelCollector); 0 <= h && this.chart.labelCollectors.splice(h,
                        1)
                }
            }); b(a, "afterGetPosition", function (h) { this.axis.getPosition && w(h.pos, this.axis.getPosition(this.pos)) }); b(a, "afterGetLabelPosition", function (h) {
                var g = this.axis, e = this.label, k = e.getBBox(), d = g.options.labels, p = d.y, q = 20, r = d.align, b = (g.translate(this.pos) + g.startAngleRad + Math.PI / 2) / Math.PI * 180 % 360, a = Math.round(b), n = "end", c = 0 > a ? a + 360 : a, x = c, t = 0, u = 0, v = null === d.y ? .3 * -k.height : 0; if (g.isRadial) {
                    var l = g.getPosition(this.pos, g.center[2] / 2 + m(y(d.distance, -25), g.center[2] / 2, -g.center[2] / 2)); "auto" === d.rotation ?
                        e.attr({ rotation: b }) : null === p && (p = g.chart.renderer.fontMetrics(e.styles && e.styles.fontSize).b - k.height / 2); null === r && (g.isCircular ? (k.width > g.len * g.tickInterval / (g.max - g.min) && (q = 0), r = b > q && b < 180 - q ? "left" : b > 180 + q && b < 360 - q ? "right" : "center") : r = "center", e.attr({ align: r })); if ("auto" === r && 2 === g.tickPositions.length && g.isCircular) {
                        90 < c && 180 > c ? c = 180 - c : 270 < c && 360 >= c && (c = 540 - c); 180 < x && 360 >= x && (x = 360 - x); if (g.pane.options.startAngle === a || g.pane.options.startAngle === a + 360 || g.pane.options.startAngle === a - 360) n = "start";
                            r = -90 <= a && 90 >= a || -360 <= a && -270 >= a || 270 <= a && 360 >= a ? "start" === n ? "right" : "left" : "start" === n ? "left" : "right"; 70 < x && 110 > x && (r = "center"); 15 > c || 180 <= c && 195 > c ? t = .3 * k.height : 15 <= c && 35 >= c ? t = "start" === n ? 0 : .75 * k.height : 195 <= c && 215 >= c ? t = "start" === n ? .75 * k.height : 0 : 35 < c && 90 >= c ? t = "start" === n ? .25 * -k.height : k.height : 215 < c && 270 >= c && (t = "start" === n ? k.height : .25 * -k.height); 15 > x ? u = "start" === n ? .15 * -k.height : .15 * k.height : 165 < x && 180 >= x && (u = "start" === n ? .15 * k.height : .15 * -k.height); e.attr({ align: r }); e.translate(u, t + v)
                        } h.pos.x =
                            l.x + d.x; h.pos.y = l.y + p
                }
            }); c(A, "getMarkPath", function (h, g, e, k, d, p, q) { var r = this.axis; r.isRadial ? (h = r.getPosition(this.pos, r.center[2] / 2 + k), g = ["M", g, e, "L", h.x, h.y]) : g = h.call(this, g, e, k, d, p, q); return g })
    }); E(f, "parts-more/AreaRangeSeries.js", [f["parts/Globals.js"], f["parts/Point.js"], f["parts/Utilities.js"]], function (l, a, c) {
        var b = c.defined, u = c.extend, v = c.isArray, w = c.isNumber, f = c.pick; c = c.seriesType; var y = l.seriesTypes, d = l.Series.prototype, m = a.prototype; c("arearange", "area", {
            lineWidth: 1, threshold: null,
            tooltip: { pointFormat: '<span style="color:{series.color}">\u25cf</span> {series.name}: <b>{point.low}</b> - <b>{point.high}</b><br/>' }, trackByArea: !0, dataLabels: { align: null, verticalAlign: null, xLow: 0, xHigh: 0, yLow: 0, yHigh: 0 }
        }, {
            pointArrayMap: ["low", "high"], pointValKey: "low", deferTranslatePolar: !0, toYData: function (d) { return [d.low, d.high] }, highToXY: function (d) {
                var b = this.chart, a = this.xAxis.postTranslate(d.rectPlotX, this.yAxis.len - d.plotHigh); d.plotHighX = a.x - b.plotLeft; d.plotHigh = a.y - b.plotTop; d.plotLowX =
                    d.plotX
            }, translate: function () { var d = this, b = d.yAxis, a = !!d.modifyValue; y.area.prototype.translate.apply(d); d.points.forEach(function (c) { var r = c.high, p = c.plotY; c.isNull ? c.plotY = null : (c.plotLow = p, c.plotHigh = b.translate(a ? d.modifyValue(r, c) : r, 0, 1, 0, 1), a && (c.yBottom = c.plotHigh)) }); this.chart.polar && this.points.forEach(function (b) { d.highToXY(b); b.tooltipPos = [(b.plotHighX + b.plotLowX) / 2, (b.plotHigh + b.plotLow) / 2] }) }, getGraphPath: function (d) {
                var b = [], a = [], c, r = y.area.prototype.getGraphPath; var p = this.options;
                var h = this.chart.polar && !1 !== p.connectEnds, g = p.connectNulls, e = p.step; d = d || this.points; for (c = d.length; c--;) { var k = d[c]; k.isNull || h || g || d[c + 1] && !d[c + 1].isNull || a.push({ plotX: k.plotX, plotY: k.plotY, doCurve: !1 }); var B = { polarPlotY: k.polarPlotY, rectPlotX: k.rectPlotX, yBottom: k.yBottom, plotX: f(k.plotHighX, k.plotX), plotY: k.plotHigh, isNull: k.isNull }; a.push(B); b.push(B); k.isNull || h || g || d[c - 1] && !d[c - 1].isNull || a.push({ plotX: k.plotX, plotY: k.plotY, doCurve: !1 }) } d = r.call(this, d); e && (!0 === e && (e = "left"), p.step = {
                    left: "right",
                    center: "center", right: "left"
                }[e]); b = r.call(this, b); a = r.call(this, a); p.step = e; p = [].concat(d, b); this.chart.polar || "M" !== a[0] || (a[0] = "L"); this.graphPath = p; this.areaPath = d.concat(a); p.isArea = !0; p.xMap = d.xMap; this.areaPath.xMap = d.xMap; return p
            }, drawDataLabels: function () {
                var b = this.points, a = b.length, c, m = [], r = this.options.dataLabels, p, h = this.chart.inverted; if (v(r)) if (1 < r.length) { var g = r[0]; var e = r[1] } else g = r[0], e = { enabled: !1 }; else g = u({}, r), g.x = r.xHigh, g.y = r.yHigh, e = u({}, r), e.x = r.xLow, e.y = r.yLow; if (g.enabled ||
                    this._hasPointLabels) { for (c = a; c--;)if (p = b[c]) { var k = g.inside ? p.plotHigh < p.plotLow : p.plotHigh > p.plotLow; p.y = p.high; p._plotY = p.plotY; p.plotY = p.plotHigh; m[c] = p.dataLabel; p.dataLabel = p.dataLabelUpper; p.below = k; h ? g.align || (g.align = k ? "right" : "left") : g.verticalAlign || (g.verticalAlign = k ? "top" : "bottom") } this.options.dataLabels = g; d.drawDataLabels && d.drawDataLabels.apply(this, arguments); for (c = a; c--;)if (p = b[c]) p.dataLabelUpper = p.dataLabel, p.dataLabel = m[c], delete p.dataLabels, p.y = p.low, p.plotY = p._plotY } if (e.enabled ||
                        this._hasPointLabels) { for (c = a; c--;)if (p = b[c]) k = e.inside ? p.plotHigh < p.plotLow : p.plotHigh > p.plotLow, p.below = !k, h ? e.align || (e.align = k ? "left" : "right") : e.verticalAlign || (e.verticalAlign = k ? "bottom" : "top"); this.options.dataLabels = e; d.drawDataLabels && d.drawDataLabels.apply(this, arguments) } if (g.enabled) for (c = a; c--;)if (p = b[c]) p.dataLabels = [p.dataLabelUpper, p.dataLabel].filter(function (e) { return !!e }); this.options.dataLabels = r
            }, alignDataLabel: function () { y.column.prototype.alignDataLabel.apply(this, arguments) },
                drawPoints: function () {
                    var a = this.points.length, c; d.drawPoints.apply(this, arguments); for (c = 0; c < a;) {
                        var m = this.points[c]; m.origProps = { plotY: m.plotY, plotX: m.plotX, isInside: m.isInside, negative: m.negative, zone: m.zone, y: m.y }; m.lowerGraphic = m.graphic; m.graphic = m.upperGraphic; m.plotY = m.plotHigh; b(m.plotHighX) && (m.plotX = m.plotHighX); m.y = m.high; m.negative = m.high < (this.options.threshold || 0); m.zone = this.zones.length && m.getZone(); this.chart.polar || (m.isInside = m.isTopInside = "undefined" !== typeof m.plotY && 0 <= m.plotY &&
                            m.plotY <= this.yAxis.len && 0 <= m.plotX && m.plotX <= this.xAxis.len); c++
                    } d.drawPoints.apply(this, arguments); for (c = 0; c < a;)m = this.points[c], m.upperGraphic = m.graphic, m.graphic = m.lowerGraphic, u(m, m.origProps), delete m.origProps, c++
                }, setStackedPoints: l.noop
            }, {
                setState: function () {
                    var d = this.state, a = this.series, c = a.chart.polar; b(this.plotHigh) || (this.plotHigh = a.yAxis.toPixels(this.high, !0)); b(this.plotLow) || (this.plotLow = this.plotY = a.yAxis.toPixels(this.low, !0)); a.stateMarkerGraphic && (a.lowerStateMarkerGraphic =
                        a.stateMarkerGraphic, a.stateMarkerGraphic = a.upperStateMarkerGraphic); this.graphic = this.upperGraphic; this.plotY = this.plotHigh; c && (this.plotX = this.plotHighX); m.setState.apply(this, arguments); this.state = d; this.plotY = this.plotLow; this.graphic = this.lowerGraphic; c && (this.plotX = this.plotLowX); a.stateMarkerGraphic && (a.upperStateMarkerGraphic = a.stateMarkerGraphic, a.stateMarkerGraphic = a.lowerStateMarkerGraphic, a.lowerStateMarkerGraphic = void 0); m.setState.apply(this, arguments)
                }, haloPath: function () {
                    var d = this.series.chart.polar,
                    a = []; this.plotY = this.plotLow; d && (this.plotX = this.plotLowX); this.isInside && (a = m.haloPath.apply(this, arguments)); this.plotY = this.plotHigh; d && (this.plotX = this.plotHighX); this.isTopInside && (a = a.concat(m.haloPath.apply(this, arguments))); return a
                }, destroyElements: function () { ["lowerGraphic", "upperGraphic"].forEach(function (d) { this[d] && (this[d] = this[d].destroy()) }, this); this.graphic = null; return m.destroyElements.apply(this, arguments) }, isValid: function () { return w(this.low) && w(this.high) }
            }); ""
    }); E(f, "parts-more/AreaSplineRangeSeries.js",
        [f["parts/Globals.js"], f["parts/Utilities.js"]], function (l, a) { a = a.seriesType; a("areasplinerange", "arearange", null, { getPointSpline: l.seriesTypes.spline.prototype.getPointSpline }); "" }); E(f, "parts-more/ColumnRangeSeries.js", [f["parts/Globals.js"], f["parts/Utilities.js"]], function (l, a) {
            var c = a.clamp, b = a.merge, u = a.pick; a = a.seriesType; var v = l.defaultPlotOptions, w = l.noop, f = l.seriesTypes.column.prototype; a("columnrange", "arearange", b(v.column, v.arearange, { pointRange: null, marker: null, states: { hover: { halo: !1 } } }),
                {
                    translate: function () {
                        var a = this, d = a.yAxis, b = a.xAxis, n = b.startAngleRad, v, l = a.chart, w = a.xAxis.isRadial, r = Math.max(l.chartWidth, l.chartHeight) + 999, p; f.translate.apply(a); a.points.forEach(function (h) {
                            var g = h.shapeArgs, e = a.options.minPointLength; h.plotHigh = p = c(d.translate(h.high, 0, 1, 0, 1), -r, r); h.plotLow = c(h.plotY, -r, r); var k = p; var B = u(h.rectPlotY, h.plotY) - p; Math.abs(B) < e ? (e -= B, B += e, k -= e / 2) : 0 > B && (B *= -1, k -= B); w ? (v = h.barX + n, h.shapeType = "arc", h.shapeArgs = a.polarArc(k + B, k, v, v + h.pointWidth)) : (g.height = B, g.y =
                                k, h.tooltipPos = l.inverted ? [d.len + d.pos - l.plotLeft - k - B / 2, b.len + b.pos - l.plotTop - g.x - g.width / 2, B] : [b.left - l.plotLeft + g.x + g.width / 2, d.pos - l.plotTop + k + B / 2, B])
                        })
                    }, directTouch: !0, trackerGroups: ["group", "dataLabelsGroup"], drawGraph: w, getSymbol: w, crispCol: function () { return f.crispCol.apply(this, arguments) }, drawPoints: function () { return f.drawPoints.apply(this, arguments) }, drawTracker: function () { return f.drawTracker.apply(this, arguments) }, getColumnMetrics: function () { return f.getColumnMetrics.apply(this, arguments) },
                    pointAttribs: function () { return f.pointAttribs.apply(this, arguments) }, animate: function () { return f.animate.apply(this, arguments) }, polarArc: function () { return f.polarArc.apply(this, arguments) }, translate3dPoints: function () { return f.translate3dPoints.apply(this, arguments) }, translate3dShapes: function () { return f.translate3dShapes.apply(this, arguments) }
                }, { setState: f.pointClass.prototype.setState }); ""
        }); E(f, "parts-more/ColumnPyramidSeries.js", [f["parts/Globals.js"], f["parts/Utilities.js"]], function (l, a) {
            var c =
                a.clamp, b = a.pick; a = a.seriesType; var u = l.seriesTypes.column.prototype; a("columnpyramid", "column", {}, {
                    translate: function () {
                        var a = this, l = a.chart, f = a.options, y = a.dense = 2 > a.closestPointRange * a.xAxis.transA; y = a.borderWidth = b(f.borderWidth, y ? 0 : 1); var d = a.yAxis, m = f.threshold, n = a.translatedThreshold = d.getThreshold(m), t = b(f.minPointLength, 5), x = a.getColumnMetrics(), A = x.width, r = a.barW = Math.max(A, 1 + 2 * y), p = a.pointXOffset = x.offset; l.inverted && (n -= .5); f.pointPadding && (r = Math.ceil(r)); u.translate.apply(a); a.points.forEach(function (h) {
                            var g =
                                b(h.yBottom, n), e = 999 + Math.abs(g), k = c(h.plotY, -e, d.len + e); e = h.plotX + p; var B = r / 2, C = Math.min(k, g); g = Math.max(k, g) - C; var q; h.barX = e; h.pointWidth = A; h.tooltipPos = l.inverted ? [d.len + d.pos - l.plotLeft - k, a.xAxis.len - e - B, g] : [e + B, k + d.pos - l.plotTop, g]; k = m + (h.total || h.y); "percent" === f.stacking && (k = m + (0 > h.y) ? -100 : 100); k = d.toPixels(k, !0); var F = (q = l.plotHeight - k - (l.plotHeight - n)) ? B * (C - k) / q : 0; var G = q ? B * (C + g - k) / q : 0; q = e - F + B; F = e + F + B; var u = e + G + B; G = e - G + B; var v = C - t; var w = C + g; 0 > h.y && (v = C, w = C + g + t); l.inverted && (u = l.plotWidth -
                                    C, q = k - (l.plotWidth - n), F = B * (k - u) / q, G = B * (k - (u - g)) / q, q = e + B + F, F = q - 2 * F, u = e - G + B, G = e + G + B, v = C, w = C + g - t, 0 > h.y && (w = C + g + t)); h.shapeType = "path"; h.shapeArgs = { x: q, y: v, width: F - q, height: g, d: ["M", q, v, "L", F, v, u, w, G, w, "Z"] }
                        })
                    }
                }); ""
        }); E(f, "parts-more/GaugeSeries.js", [f["parts/Globals.js"], f["parts/Utilities.js"]], function (l, a) {
            var c = a.clamp, b = a.isNumber, u = a.merge, v = a.pick, f = a.pInt; a = a.seriesType; var z = l.Series, y = l.TrackerMixin; a("gauge", "line", {
                dataLabels: {
                    borderColor: "#cccccc", borderRadius: 3, borderWidth: 1, crop: !1, defer: !1,
                    enabled: !0, verticalAlign: "top", y: 15, zIndex: 2
                }, dial: {}, pivot: {}, tooltip: { headerFormat: "" }, showInLegend: !1
            }, {
                angular: !0, directTouch: !0, drawGraph: l.noop, fixedBox: !0, forceDL: !0, noSharedTooltip: !0, trackerGroups: ["group", "dataLabelsGroup"], translate: function () {
                    var d = this.yAxis, a = this.options, n = d.center; this.generatePoints(); this.points.forEach(function (m) {
                        var l = u(a.dial, m.dial), t = f(v(l.radius, "80%")) * n[2] / 200, r = f(v(l.baseLength, "70%")) * t / 100, p = f(v(l.rearLength, "10%")) * t / 100, h = l.baseWidth || 3, g = l.topWidth ||
                            1, e = a.overshoot, k = d.startAngleRad + d.translate(m.y, null, null, null, !0); if (b(e) || !1 === a.wrap) e = b(e) ? e / 180 * Math.PI : 0, k = c(k, d.startAngleRad - e, d.endAngleRad + e); k = 180 * k / Math.PI; m.shapeType = "path"; m.shapeArgs = { d: l.path || ["M", -p, -h / 2, "L", r, -h / 2, t, -g / 2, t, g / 2, r, h / 2, -p, h / 2, "z"], translateX: n[0], translateY: n[1], rotation: k }; m.plotX = n[0]; m.plotY = n[1]
                    })
                }, drawPoints: function () {
                    var d = this, a = d.chart, b = d.yAxis.center, c = d.pivot, l = d.options, f = l.pivot, r = a.renderer; d.points.forEach(function (b) {
                        var h = b.graphic, g = b.shapeArgs,
                        e = g.d, k = u(l.dial, b.dial); h ? (h.animate(g), g.d = e) : b.graphic = r[b.shapeType](g).attr({ rotation: g.rotation, zIndex: 1 }).addClass("highcharts-dial").add(d.group); if (!a.styledMode) b.graphic[h ? "animate" : "attr"]({ stroke: k.borderColor || "none", "stroke-width": k.borderWidth || 0, fill: k.backgroundColor || "#000000" })
                    }); c ? c.animate({ translateX: b[0], translateY: b[1] }) : (d.pivot = r.circle(0, 0, v(f.radius, 5)).attr({ zIndex: 2 }).addClass("highcharts-pivot").translate(b[0], b[1]).add(d.group), a.styledMode || d.pivot.attr({
                        "stroke-width": f.borderWidth ||
                            0, stroke: f.borderColor || "#cccccc", fill: f.backgroundColor || "#000000"
                    }))
                }, animate: function (d) { var a = this; d || a.points.forEach(function (d) { var b = d.graphic; b && (b.attr({ rotation: 180 * a.yAxis.startAngleRad / Math.PI }), b.animate({ rotation: d.shapeArgs.rotation }, a.options.animation)) }) }, render: function () { this.group = this.plotGroup("group", "series", this.visible ? "visible" : "hidden", this.options.zIndex, this.chart.seriesGroup); z.prototype.render.call(this); this.group.clip(this.chart.clipRect) }, setData: function (d, a) {
                    z.prototype.setData.call(this,
                        d, !1); this.processData(); this.generatePoints(); v(a, !0) && this.chart.redraw()
                }, hasData: function () { return !!this.points.length }, drawTracker: y && y.drawTrackerPoint
                }, { setState: function (d) { this.state = d } }); ""
        }); E(f, "parts-more/BoxPlotSeries.js", [f["parts/Globals.js"], f["parts/Utilities.js"]], function (l, a) {
            var c = a.pick; a = a.seriesType; var b = l.noop, u = l.seriesTypes; a("boxplot", "column", {
                threshold: null, tooltip: { pointFormat: '<span style="color:{point.color}">\u25cf</span> <b> {series.name}</b><br/>Maximum: {point.high}<br/>Upper quartile: {point.q3}<br/>Median: {point.median}<br/>Lower quartile: {point.q1}<br/>Minimum: {point.low}<br/>' },
                whiskerLength: "50%", fillColor: "#ffffff", lineWidth: 1, medianWidth: 2, whiskerWidth: 2
            }, {
                pointArrayMap: ["low", "q1", "median", "q3", "high"], toYData: function (a) { return [a.low, a.q1, a.median, a.q3, a.high] }, pointValKey: "high", pointAttribs: function () { return {} }, drawDataLabels: b, translate: function () { var a = this.yAxis, b = this.pointArrayMap; u.column.prototype.translate.apply(this); this.points.forEach(function (c) { b.forEach(function (b) { null !== c[b] && (c[b + "Plot"] = a.translate(c[b], 0, 1, 0, 1)) }); c.plotHigh = c.highPlot }) }, drawPoints: function () {
                    var a =
                        this, b = a.options, l = a.chart, u = l.renderer, d, m, n, f, x, A, r = 0, p, h, g, e, k = !1 !== a.doQuartiles, B, C = a.options.whiskerLength; a.points.forEach(function (q) {
                            var F = q.graphic, G = F ? "animate" : "attr", K = q.shapeArgs, v = {}, t = {}, H = {}, J = {}, I = q.color || a.color; "undefined" !== typeof q.plotY && (p = K.width, h = Math.floor(K.x), g = h + p, e = Math.round(p / 2), d = Math.floor(k ? q.q1Plot : q.lowPlot), m = Math.floor(k ? q.q3Plot : q.lowPlot), n = Math.floor(q.highPlot), f = Math.floor(q.lowPlot), F || (q.graphic = F = u.g("point").add(a.group), q.stem = u.path().addClass("highcharts-boxplot-stem").add(F),
                                C && (q.whiskers = u.path().addClass("highcharts-boxplot-whisker").add(F)), k && (q.box = u.path(void 0).addClass("highcharts-boxplot-box").add(F)), q.medianShape = u.path(void 0).addClass("highcharts-boxplot-median").add(F)), l.styledMode || (t.stroke = q.stemColor || b.stemColor || I, t["stroke-width"] = c(q.stemWidth, b.stemWidth, b.lineWidth), t.dashstyle = q.stemDashStyle || b.stemDashStyle, q.stem.attr(t), C && (H.stroke = q.whiskerColor || b.whiskerColor || I, H["stroke-width"] = c(q.whiskerWidth, b.whiskerWidth, b.lineWidth), q.whiskers.attr(H)),
                                    k && (v.fill = q.fillColor || b.fillColor || I, v.stroke = b.lineColor || I, v["stroke-width"] = b.lineWidth || 0, q.box.attr(v)), J.stroke = q.medianColor || b.medianColor || I, J["stroke-width"] = c(q.medianWidth, b.medianWidth, b.lineWidth), q.medianShape.attr(J)), A = q.stem.strokeWidth() % 2 / 2, r = h + e + A, q.stem[G]({ d: ["M", r, m, "L", r, n, "M", r, d, "L", r, f] }), k && (A = q.box.strokeWidth() % 2 / 2, d = Math.floor(d) + A, m = Math.floor(m) + A, h += A, g += A, q.box[G]({ d: ["M", h, m, "L", h, d, "L", g, d, "L", g, m, "L", h, m, "z"] })), C && (A = q.whiskers.strokeWidth() % 2 / 2, n += A, f += A,
                                        B = /%$/.test(C) ? e * parseFloat(C) / 100 : C / 2, q.whiskers[G]({ d: ["M", r - B, n, "L", r + B, n, "M", r - B, f, "L", r + B, f] })), x = Math.round(q.medianPlot), A = q.medianShape.strokeWidth() % 2 / 2, x += A, q.medianShape[G]({ d: ["M", h, x, "L", g, x] }))
                        })
                }, setStackedPoints: b
                }); ""
        }); E(f, "parts-more/ErrorBarSeries.js", [f["parts/Globals.js"], f["parts/Utilities.js"]], function (l, a) {
            a = a.seriesType; var c = l.noop, b = l.seriesTypes; a("errorbar", "boxplot", {
                color: "#000000", grouping: !1, linkedTo: ":previous", tooltip: { pointFormat: '<span style="color:{point.color}">\u25cf</span> {series.name}: <b>{point.low}</b> - <b>{point.high}</b><br/>' },
                whiskerWidth: null
            }, { type: "errorbar", pointArrayMap: ["low", "high"], toYData: function (a) { return [a.low, a.high] }, pointValKey: "high", doQuartiles: !1, drawDataLabels: b.arearange ? function () { var a = this.pointValKey; b.arearange.prototype.drawDataLabels.call(this); this.data.forEach(function (b) { b.y = b[a] }) } : c, getColumnMetrics: function () { return this.linkedParent && this.linkedParent.columnMetrics || b.column.prototype.getColumnMetrics.call(this) } }); ""
        }); E(f, "parts-more/WaterfallSeries.js", [f["parts/Globals.js"], f["parts/Point.js"],
        f["parts/Utilities.js"]], function (l, a, c) {
            var b = c.addEvent, u = c.arrayMax, f = c.arrayMin, w = c.correctFloat, z = c.isNumber, y = c.objectEach, d = c.pick; c = c.seriesType; var m = l.Axis, n = l.Chart, t = l.Series, x = l.StackItem, A = l.seriesTypes; b(m, "afterInit", function () { this.isXAxis || (this.waterfallStacks = { changed: !1 }) }); b(m, "afterBuildStacks", function () { this.waterfallStacks.changed = !1; delete this.waterfallStacks.alreadyChanged }); b(n, "beforeRedraw", function () {
                for (var a = this.axes, d = this.series, h = d.length; h--;)d[h].options.stacking &&
                    (a.forEach(function (g) { g.isXAxis || (g.waterfallStacks.changed = !0) }), h = 0)
            }); b(m, "afterRender", function () { var a = this.options.stackLabels; a && a.enabled && this.waterfallStacks && this.renderWaterfallStackTotals() }); m.prototype.renderWaterfallStackTotals = function () {
                var a = this.waterfallStacks, d = this.stackTotalGroup, h = new x(this, this.options.stackLabels, !1, 0, void 0); this.dummyStackItem = h; y(a, function (g) {
                    y(g, function (e) {
                    h.total = e.stackTotal; e.label && (h.label = e.label); x.prototype.render.call(h, d); e.label = h.label;
                        delete h.label
                    })
                }); h.total = null
            }; c("waterfall", "column", { dataLabels: { inside: !0 }, lineWidth: 1, lineColor: "#333333", dashStyle: "Dot", borderColor: "#333333", states: { hover: { lineWidthPlus: 0 } } }, {
                pointValKey: "y", showLine: !0, generatePoints: function () { var a; A.column.prototype.generatePoints.apply(this); var d = 0; for (a = this.points.length; d < a; d++) { var h = this.points[d]; var g = this.processedYData[d]; if (h.isIntermediateSum || h.isSum) h.y = w(g) } }, translate: function () {
                    var a = this.options, b = this.yAxis, h, g = d(a.minPointLength,
                        5), e = g / 2, k = a.threshold, c = a.stacking, C = b.waterfallStacks[this.stackKey]; A.column.prototype.translate.apply(this); var q = h = k; var F = this.points; var m = 0; for (a = F.length; m < a; m++) {
                            var l = F[m]; var u = this.processedYData[m]; var n = l.shapeArgs; var f = [0, u]; var t = l.y; if (c) {
                                if (C) {
                                    f = C[m]; if ("overlap" === c) { var v = f.stackState[f.stateIndex--]; v = 0 <= t ? v : v - t; Object.hasOwnProperty.call(f, "absolutePos") && delete f.absolutePos; Object.hasOwnProperty.call(f, "absoluteNeg") && delete f.absoluteNeg } else 0 <= t ? (v = f.threshold + f.posTotal,
                                        f.posTotal -= t) : (v = f.threshold + f.negTotal, f.negTotal -= t, v -= t), !f.posTotal && Object.hasOwnProperty.call(f, "absolutePos") && (f.posTotal = f.absolutePos, delete f.absolutePos), !f.negTotal && Object.hasOwnProperty.call(f, "absoluteNeg") && (f.negTotal = f.absoluteNeg, delete f.absoluteNeg); l.isSum || (f.connectorThreshold = f.threshold + f.stackTotal); b.reversed ? (u = 0 <= t ? v - t : v + t, t = v) : (u = v, t = v - t); l.below = u <= d(k, 0); n.y = b.translate(u, 0, 1, 0, 1); n.height = Math.abs(n.y - b.translate(t, 0, 1, 0, 1))
                                } if (t = b.dummyStackItem) t.x = m, t.label =
                                    C[m].label, t.setOffset(this.pointXOffset || 0, this.barW || 0, this.stackedYNeg[m], this.stackedYPos[m])
                            } else v = Math.max(q, q + t) + f[0], n.y = b.translate(v, 0, 1, 0, 1), l.isSum ? (n.y = b.translate(f[1], 0, 1, 0, 1), n.height = Math.min(b.translate(f[0], 0, 1, 0, 1), b.len) - n.y) : l.isIntermediateSum ? (0 <= t ? (u = f[1] + h, t = h) : (u = h, t = f[1] + h), b.reversed && (u ^= t, t ^= u, u ^= t), n.y = b.translate(u, 0, 1, 0, 1), n.height = Math.abs(n.y - Math.min(b.translate(t, 0, 1, 0, 1), b.len)), h += f[1]) : (n.height = 0 < u ? b.translate(q, 0, 1, 0, 1) - n.y : b.translate(q, 0, 1, 0, 1) - b.translate(q -
                                u, 0, 1, 0, 1), q += u, l.below = q < d(k, 0)), 0 > n.height && (n.y += n.height, n.height *= -1); l.plotY = n.y = Math.round(n.y) - this.borderWidth % 2 / 2; n.height = Math.max(Math.round(n.height), .001); l.yBottom = n.y + n.height; n.height <= g && !l.isNull ? (n.height = g, n.y -= e, l.plotY = n.y, l.minPointLengthOffset = 0 > l.y ? -e : e) : (l.isNull && (n.width = 0), l.minPointLengthOffset = 0); n = l.plotY + (l.negative ? n.height : 0); this.chart.inverted ? l.tooltipPos[0] = b.len - n : l.tooltipPos[1] = n
                        }
                }, processData: function (a) {
                    var d = this.options, h = this.yData, g = d.data, e = h.length,
                    k = d.threshold || 0, b, r, q, c, m; for (m = r = b = q = c = 0; m < e; m++) { var n = h[m]; var l = g && g[m] ? g[m] : {}; "sum" === n || l.isSum ? h[m] = w(r) : "intermediateSum" === n || l.isIntermediateSum ? (h[m] = w(b), b = 0) : (r += n, b += n); q = Math.min(r, q); c = Math.max(r, c) } t.prototype.processData.call(this, a); d.stacking || (this.dataMin = q + k, this.dataMax = c)
                }, toYData: function (a) { return a.isSum ? "sum" : a.isIntermediateSum ? "intermediateSum" : a.y }, updateParallelArrays: function (a, d) {
                    t.prototype.updateParallelArrays.call(this, a, d); if ("sum" === this.yData[0] || "intermediateSum" ===
                        this.yData[0]) this.yData[0] = null
                }, pointAttribs: function (a, d) { var b = this.options.upColor; b && !a.options.color && (a.color = 0 < a.y ? b : null); a = A.column.prototype.pointAttribs.call(this, a, d); delete a.dashstyle; return a }, getGraphPath: function () { return ["M", 0, 0] }, getCrispPath: function () {
                    var a = this.data, d = this.yAxis, b = a.length, g = Math.round(this.graph.strokeWidth()) % 2 / 2, e = Math.round(this.borderWidth) % 2 / 2, k = this.xAxis.reversed, c = this.yAxis.reversed, C = this.options.stacking, q = [], m; for (m = 1; m < b; m++) {
                        var n = a[m].shapeArgs;
                        var l = a[m - 1]; var f = a[m - 1].shapeArgs; var u = d.waterfallStacks[this.stackKey]; var t = 0 < l.y ? -f.height : 0; if (u) { u = u[m - 1]; C ? (u = u.connectorThreshold, t = Math.round(d.translate(u, 0, 1, 0, 1) + (c ? t : 0)) - g) : t = f.y + l.minPointLengthOffset + e - g; var v = ["M", f.x + (k ? 0 : f.width), t, "L", n.x + (k ? n.width : 0), t] } if (!C && v && 0 > l.y && !c || 0 < l.y && c) v[2] += f.height, v[5] += f.height; q = q.concat(v)
                    } return q
                }, drawGraph: function () { t.prototype.drawGraph.call(this); this.graph.attr({ d: this.getCrispPath() }) }, setStackedPoints: function () {
                    function a(e, a, k,
                        g) { if (z) for (k; k < z; k++)w.stackState[k] += g; else w.stackState[0] = e, z = w.stackState.length; w.stackState.push(w.stackState[z - 1] + a) } var d = this.options, b = this.yAxis.waterfallStacks, g = d.threshold, e = g || 0, k = e, c = this.stackKey, C = this.xData, q = C.length, m, n, l; this.yAxis.usePercentage = !1; var f = n = l = e; if (this.visible || !this.chart.options.chart.ignoreHiddenSeries) {
                            var u = b.changed; (m = b.alreadyChanged) && 0 > m.indexOf(c) && (u = !0); b[c] || (b[c] = {}); m = b[c]; for (var t = 0; t < q; t++) {
                                var v = C[t]; if (!m[v] || u) m[v] = {
                                    negTotal: 0, posTotal: 0,
                                    stackTotal: 0, threshold: 0, stateIndex: 0, stackState: [], label: u && m[v] ? m[v].label : void 0
                                }; var w = m[v]; var x = this.yData[t]; 0 <= x ? w.posTotal += x : w.negTotal += x; var y = d.data[t]; v = w.absolutePos = w.posTotal; var A = w.absoluteNeg = w.negTotal; w.stackTotal = v + A; var z = w.stackState.length; y && y.isIntermediateSum ? (a(l, n, 0, l), l = n, n = g, e ^= k, k ^= e, e ^= k) : y && y.isSum ? (a(g, f, z), e = g) : (a(e, x, 0, f), y && (f += x, n += x)); w.stateIndex++; w.threshold = e; e += w.stackTotal
                            } b.changed = !1; b.alreadyChanged || (b.alreadyChanged = []); b.alreadyChanged.push(c)
                        }
                },
                getExtremes: function () { var a = this.options.stacking; if (a) { var d = this.yAxis; d = d.waterfallStacks; var b = this.stackedYNeg = []; var g = this.stackedYPos = []; "overlap" === a ? y(d[this.stackKey], function (e) { b.push(f(e.stackState)); g.push(u(e.stackState)) }) : y(d[this.stackKey], function (e) { b.push(e.negTotal + e.threshold); g.push(e.posTotal + e.threshold) }); this.dataMin = f(b); this.dataMax = u(g) } }
            }, {
                getClassName: function () {
                    var d = a.prototype.getClassName.call(this); this.isSum ? d += " highcharts-sum" : this.isIntermediateSum && (d +=
                        " highcharts-intermediate-sum"); return d
                }, isValid: function () { return z(this.y) || this.isSum || this.isIntermediateSum }
                }); ""
        }); E(f, "parts-more/PolygonSeries.js", [f["parts/Globals.js"], f["mixins/legend-symbol.js"], f["parts/Utilities.js"]], function (l, a, c) {
            c = c.seriesType; var b = l.Series, f = l.seriesTypes; c("polygon", "scatter", { marker: { enabled: !1, states: { hover: { enabled: !1 } } }, stickyTracking: !1, tooltip: { followPointer: !0, pointFormat: "" }, trackByArea: !0 }, {
                type: "polygon", getGraphPath: function () {
                    for (var a = b.prototype.getGraphPath.call(this),
                        c = a.length + 1; c--;)(c === a.length || "M" === a[c]) && 0 < c && a.splice(c, 0, "z"); return this.areaPath = a
                }, drawGraph: function () { this.options.fillColor = this.color; f.area.prototype.drawGraph.call(this) }, drawLegendSymbol: a.drawRectangle, drawTracker: b.prototype.drawTracker, setStackedPoints: l.noop
            }); ""
        }); E(f, "parts-more/BubbleLegend.js", [f["parts/Globals.js"], f["parts/Color.js"], f["parts/Legend.js"], f["parts/Utilities.js"]], function (l, a, c, b) {
            ""; var f = a.parse; a = b.addEvent; var v = b.arrayMax, w = b.arrayMin, z = b.isNumber, y =
                b.merge, d = b.objectEach, m = b.pick, n = b.stableSort, t = b.wrap, x = l.Series, A = l.Chart, r = l.noop, p = l.setOptions; p({
                    legend: {
                        bubbleLegend: {
                            borderColor: void 0, borderWidth: 2, className: void 0, color: void 0, connectorClassName: void 0, connectorColor: void 0, connectorDistance: 60, connectorWidth: 1, enabled: !1, labels: { className: void 0, allowOverlap: !1, format: "", formatter: void 0, align: "right", style: { fontSize: 10, color: void 0 }, x: 0, y: 0 }, maxSize: 60, minSize: 10, legendIndex: 0, ranges: { value: void 0, borderColor: void 0, color: void 0, connectorColor: void 0 },
                            sizeBy: "area", sizeByAbsoluteValue: !1, zIndex: 1, zThreshold: 0
                        }
                    }
                }); p = function () {
                    function a(a, e) { this.options = this.symbols = this.visible = this.ranges = this.movementX = this.maxLabel = this.legendSymbol = this.legendItemWidth = this.legendItemHeight = this.legendItem = this.legendGroup = this.legend = this.fontMetrics = this.chart = void 0; this.setState = r; this.init(a, e) } a.prototype.init = function (a, e) { this.options = a; this.visible = !0; this.chart = e.chart; this.legend = e }; a.prototype.addToLegend = function (a) {
                        a.splice(this.options.legendIndex,
                            0, this)
                    }; a.prototype.drawLegendSymbol = function (a) {
                        var e = this.chart, k = this.options, d = m(a.options.itemDistance, 20), b = k.ranges; var g = k.connectorDistance; this.fontMetrics = e.renderer.fontMetrics(k.labels.style.fontSize.toString() + "px"); b && b.length && z(b[0].value) ? (n(b, function (e, a) { return a.value - e.value }), this.ranges = b, this.setOptions(), this.render(), e = this.getMaxLabelSize(), b = this.ranges[0].radius, a = 2 * b, g = g - b + e.width, g = 0 < g ? g : 0, this.maxLabel = e, this.movementX = "left" === k.labels.align ? g : 0, this.legendItemWidth =
                            a + g + d, this.legendItemHeight = a + this.fontMetrics.h / 2) : a.options.bubbleLegend.autoRanges = !0
                    }; a.prototype.setOptions = function () {
                        var a = this.ranges, e = this.options, k = this.chart.series[e.seriesIndex], d = this.legend.baseline, b = { "z-index": e.zIndex, "stroke-width": e.borderWidth }, h = { "z-index": e.zIndex, "stroke-width": e.connectorWidth }, c = this.getLabelStyles(), p = k.options.marker.fillOpacity, r = this.chart.styledMode; a.forEach(function (g, q) {
                            r || (b.stroke = m(g.borderColor, e.borderColor, k.color), b.fill = m(g.color, e.color,
                                1 !== p ? f(k.color).setOpacity(p).get("rgba") : k.color), h.stroke = m(g.connectorColor, e.connectorColor, k.color)); a[q].radius = this.getRangeRadius(g.value); a[q] = y(a[q], { center: a[0].radius - a[q].radius + d }); r || y(!0, a[q], { bubbleStyle: y(!1, b), connectorStyle: y(!1, h), labelStyle: c })
                        }, this)
                    }; a.prototype.getLabelStyles = function () {
                        var a = this.options, e = {}, k = "left" === a.labels.align, b = this.legend.options.rtl; d(a.labels.style, function (a, k) { "color" !== k && "fontSize" !== k && "z-index" !== k && (e[k] = a) }); return y(!1, e, {
                            "font-size": a.labels.style.fontSize,
                            fill: m(a.labels.style.color, "#000000"), "z-index": a.zIndex, align: b || k ? "right" : "left"
                        })
                    }; a.prototype.getRangeRadius = function (a) { var e = this.options; return this.chart.series[this.options.seriesIndex].getRadius.call(this, e.ranges[e.ranges.length - 1].value, e.ranges[0].value, e.minSize, e.maxSize, a) }; a.prototype.render = function () {
                        var a = this.chart.renderer, e = this.options.zThreshold; this.symbols || (this.symbols = { connectors: [], bubbleItems: [], labels: [] }); this.legendSymbol = a.g("bubble-legend"); this.legendItem = a.g("bubble-legend-item");
                        this.legendSymbol.translateX = 0; this.legendSymbol.translateY = 0; this.ranges.forEach(function (a) { a.value >= e && this.renderRange(a) }, this); this.legendSymbol.add(this.legendItem); this.legendItem.add(this.legendGroup); this.hideOverlappingLabels()
                    }; a.prototype.renderRange = function (a) {
                        var e = this.options, k = e.labels, b = this.chart.renderer, d = this.symbols, g = d.labels, h = a.center, c = Math.abs(a.radius), p = e.connectorDistance, r = k.align, m = k.style.fontSize; p = this.legend.options.rtl || "left" === r ? -p : p; k = e.connectorWidth; var n =
                            this.ranges[0].radius, l = h - c - e.borderWidth / 2 + k / 2; m = m / 2 - (this.fontMetrics.h - m) / 2; var f = b.styledMode; "center" === r && (p = 0, e.connectorDistance = 0, a.labelStyle.align = "center"); r = l + e.labels.y; var u = n + p + e.labels.x; d.bubbleItems.push(b.circle(n, h + ((l % 1 ? 1 : .5) - (k % 2 ? 0 : .5)), c).attr(f ? {} : a.bubbleStyle).addClass((f ? "highcharts-color-" + this.options.seriesIndex + " " : "") + "highcharts-bubble-legend-symbol " + (e.className || "")).add(this.legendSymbol)); d.connectors.push(b.path(b.crispLine(["M", n, l, "L", n + p, l], e.connectorWidth)).attr(f ?
                                {} : a.connectorStyle).addClass((f ? "highcharts-color-" + this.options.seriesIndex + " " : "") + "highcharts-bubble-legend-connectors " + (e.connectorClassName || "")).add(this.legendSymbol)); a = b.text(this.formatLabel(a), u, r + m).attr(f ? {} : a.labelStyle).addClass("highcharts-bubble-legend-labels " + (e.labels.className || "")).add(this.legendSymbol); g.push(a); a.placed = !0; a.alignAttr = { x: u, y: r + m }
                    }; a.prototype.getMaxLabelSize = function () {
                        var a, e; this.symbols.labels.forEach(function (k) {
                            e = k.getBBox(!0); a = a ? e.width > a.width ? e :
                                a : e
                        }); return a || {}
                    }; a.prototype.formatLabel = function (a) { var e = this.options, k = e.labels.formatter; e = e.labels.format; var d = this.chart.numberFormatter; return e ? b.format(e, a) : k ? k.call(a) : d(a.value, 1) }; a.prototype.hideOverlappingLabels = function () { var a = this.chart, e = this.symbols; !this.options.labels.allowOverlap && e && (a.hideOverlappingLabels(e.labels), e.labels.forEach(function (a, b) { a.newOpacity ? a.newOpacity !== a.oldOpacity && e.connectors[b].show() : e.connectors[b].hide() })) }; a.prototype.getRanges = function () {
                        var a =
                            this.legend.bubbleLegend, e = a.options.ranges, k, b = Number.MAX_VALUE, d = -Number.MAX_VALUE; a.chart.series.forEach(function (e) { e.isBubble && !e.ignoreSeries && (k = e.zData.filter(z), k.length && (b = m(e.options.zMin, Math.min(b, Math.max(w(k), !1 === e.options.displayNegative ? e.options.zThreshold : -Number.MAX_VALUE))), d = m(e.options.zMax, Math.max(d, v(k))))) }); var h = b === d ? [{ value: d }] : [{ value: b }, { value: (b + d) / 2 }, { value: d, autoRanges: !0 }]; e.length && e[0].radius && h.reverse(); h.forEach(function (a, b) { e && e[b] && (h[b] = y(!1, e[b], a)) });
                        return h
                    }; a.prototype.predictBubbleSizes = function () { var a = this.chart, e = this.fontMetrics, b = a.legend.options, d = "horizontal" === b.layout, h = d ? a.legend.lastLineHeight : 0, q = a.plotSizeX, c = a.plotSizeY, p = a.series[this.options.seriesIndex]; a = Math.ceil(p.minPxSize); var r = Math.ceil(p.maxPxSize); p = p.options.maxSize; var m = Math.min(c, q); if (b.floating || !/%$/.test(p)) e = r; else if (p = parseFloat(p), e = (m + h - e.h / 2) * p / 100 / (p / 100 + 1), d && c - e >= q || !d && q - e >= c) e = r; return [a, Math.ceil(e)] }; a.prototype.updateRanges = function (a, e) {
                        var b =
                            this.legend.options.bubbleLegend; b.minSize = a; b.maxSize = e; b.ranges = this.getRanges()
                    }; a.prototype.correctSizes = function () { var a = this.legend, e = this.chart.series[this.options.seriesIndex]; 1 < Math.abs(Math.ceil(e.maxPxSize) - this.options.maxSize) && (this.updateRanges(this.options.minSize, e.maxPxSize), a.render()) }; return a
                }(); a(c, "afterGetAllItems", function (a) {
                    var b = this.bubbleLegend, e = this.options, d = e.bubbleLegend, h = this.chart.getVisibleBubbleSeriesIndex(); b && b.ranges && b.ranges.length && (d.ranges.length &&
                        (d.autoRanges = !!d.ranges[0].autoRanges), this.destroyItem(b)); 0 <= h && e.enabled && d.enabled && (d.seriesIndex = h, this.bubbleLegend = new l.BubbleLegend(d, this), this.bubbleLegend.addToLegend(a.allItems))
                }); A.prototype.getVisibleBubbleSeriesIndex = function () { for (var a = this.series, b = 0; b < a.length;) { if (a[b] && a[b].isBubble && a[b].visible && a[b].zData.length) return b; b++ } return -1 }; c.prototype.getLinesHeights = function () {
                    var a = this.allItems, b = [], e = a.length, d, c = 0; for (d = 0; d < e; d++)if (a[d].legendItemHeight && (a[d].itemHeight =
                        a[d].legendItemHeight), a[d] === a[e - 1] || a[d + 1] && a[d]._legendItemPos[1] !== a[d + 1]._legendItemPos[1]) { b.push({ height: 0 }); var p = b[b.length - 1]; for (c; c <= d; c++)a[c].itemHeight > p.height && (p.height = a[c].itemHeight); p.step = d } return b
                }; c.prototype.retranslateItems = function (a) {
                    var b, e, d, h = this.options.rtl, c = 0; this.allItems.forEach(function (k, g) {
                        b = k.legendGroup.translateX; e = k._legendItemPos[1]; if ((d = k.movementX) || h && k.ranges) d = h ? b - k.options.maxSize / 2 : b + d, k.legendGroup.attr({ translateX: d }); g > a[c].step && c++; k.legendGroup.attr({
                            translateY: Math.round(e +
                                a[c].height / 2)
                        }); k._legendItemPos[1] = e + a[c].height / 2
                    })
                }; a(x, "legendItemClick", function () { var a = this.chart, b = this.visible, e = this.chart.legend; e && e.bubbleLegend && (this.visible = !b, this.ignoreSeries = b, a = 0 <= a.getVisibleBubbleSeriesIndex(), e.bubbleLegend.visible !== a && (e.update({ bubbleLegend: { enabled: a } }), e.bubbleLegend.visible = a), this.visible = b) }); t(A.prototype, "drawChartBox", function (a, b, e) {
                    var k = this.legend, h = 0 <= this.getVisibleBubbleSeriesIndex(); if (k && k.options.enabled && k.bubbleLegend && k.options.bubbleLegend.autoRanges &&
                        h) { var g = k.bubbleLegend.options; h = k.bubbleLegend.predictBubbleSizes(); k.bubbleLegend.updateRanges(h[0], h[1]); g.placed || (k.group.placed = !1, k.allItems.forEach(function (e) { e.legendGroup.translateY = null })); k.render(); this.getMargins(); this.axes.forEach(function (e) { e.visible && e.render(); g.placed || (e.setScale(), e.updateNames(), d(e.ticks, function (e) { e.isNew = !0; e.isNewLabel = !0 })) }); g.placed = !0; this.getMargins(); a.call(this, b, e); k.bubbleLegend.correctSizes(); k.retranslateItems(k.getLinesHeights()) } else a.call(this,
                            b, e), k && k.options.enabled && k.bubbleLegend && (k.render(), k.retranslateItems(k.getLinesHeights()))
                }); l.BubbleLegend = p; return l.BubbleLegend
        }); E(f, "parts-more/BubbleSeries.js", [f["parts/Globals.js"], f["parts/Color.js"], f["parts/Point.js"], f["parts/Utilities.js"]], function (l, a, c, b) {
            var f = a.parse, v = b.arrayMax, w = b.arrayMin, z = b.clamp, y = b.extend, d = b.isNumber, m = b.pick, n = b.pInt; a = b.seriesType; b = l.Axis; var t = l.noop, x = l.Series, A = l.seriesTypes; a("bubble", "scatter", {
                dataLabels: {
                    formatter: function () { return this.point.z },
                    inside: !0, verticalAlign: "middle"
                }, animationLimit: 250, marker: { lineColor: null, lineWidth: 1, fillOpacity: .5, radius: null, states: { hover: { radiusPlus: 0 } }, symbol: "circle" }, minSize: 8, maxSize: "20%", softThreshold: !1, states: { hover: { halo: { size: 5 } } }, tooltip: { pointFormat: "({point.x}, {point.y}), Size: {point.z}" }, turboThreshold: 0, zThreshold: 0, zoneAxis: "z"
            }, {
                pointArrayMap: ["y", "z"], parallelArrays: ["x", "y", "z"], trackerGroups: ["group", "dataLabelsGroup"], specialGroup: "group", bubblePadding: !0, zoneAxis: "z", directTouch: !0,
                    isBubble: !0, pointAttribs: function (a, b) { var d = this.options.marker.fillOpacity; a = x.prototype.pointAttribs.call(this, a, b); 1 !== d && (a.fill = f(a.fill).setOpacity(d).get("rgba")); return a }, getRadii: function (a, b, d) { var g = this.zData, e = this.yData, k = d.minPxSize, h = d.maxPxSize, c = []; var q = 0; for (d = g.length; q < d; q++) { var p = g[q]; c.push(this.getRadius(a, b, k, h, p, e[q])) } this.radii = c }, getRadius: function (a, b, h, g, e, k) {
                        var c = this.options, p = "width" !== c.sizeBy, q = c.zThreshold, r = b - a, m = .5; if (null === k || null === e) return null; if (d(e)) {
                        c.sizeByAbsoluteValue &&
                            (e = Math.abs(e - q), r = Math.max(b - q, Math.abs(a - q)), a = 0); if (e < a) return h / 2 - 1; 0 < r && (m = (e - a) / r)
                        } p && 0 <= m && (m = Math.sqrt(m)); return Math.ceil(h + m * (g - h)) / 2
                    }, animate: function (a) { !a && this.points.length < this.options.animationLimit && this.points.forEach(function (a) { var b = a.graphic; if (b && b.width) { var d = { x: b.x, y: b.y, width: b.width, height: b.height }; b.attr({ x: a.plotX, y: a.plotY, width: 1, height: 1 }); b.animate(d, this.options.animation) } }, this) }, hasData: function () { return !!this.processedXData.length }, translate: function () {
                        var a,
                        b = this.data, c = this.radii; A.scatter.prototype.translate.call(this); for (a = b.length; a--;) { var g = b[a]; var e = c ? c[a] : 0; d(e) && e >= this.minPxSize / 2 ? (g.marker = y(g.marker, { radius: e, width: 2 * e, height: 2 * e }), g.dlBox = { x: g.plotX - e, y: g.plotY - e, width: 2 * e, height: 2 * e }) : g.shapeArgs = g.plotY = g.dlBox = void 0 }
                    }, alignDataLabel: A.column.prototype.alignDataLabel, buildKDTree: t, applyZones: t
                }, { haloPath: function (a) { return c.prototype.haloPath.call(this, 0 === a ? 0 : (this.marker ? this.marker.radius || 0 : 0) + a) }, ttBelow: !1 }); b.prototype.beforePadding =
                    function () {
                        var a = this, b = this.len, c = this.chart, g = 0, e = b, k = this.isXAxis, l = k ? "xData" : "yData", f = this.min, q = {}, u = Math.min(c.plotWidth, c.plotHeight), t = Number.MAX_VALUE, x = -Number.MAX_VALUE, y = this.max - f, A = b / y, H = []; this.series.forEach(function (e) {
                            var b = e.options; !e.bubblePadding || !e.visible && c.options.chart.ignoreHiddenSeries || (a.allowZoomOutside = !0, H.push(e), k && (["minSize", "maxSize"].forEach(function (e) { var a = b[e], d = /%$/.test(a); a = n(a); q[e] = d ? u * a / 100 : a }), e.minPxSize = q.minSize, e.maxPxSize = Math.max(q.maxSize,
                                q.minSize), e = e.zData.filter(d), e.length && (t = m(b.zMin, z(w(e), !1 === b.displayNegative ? b.zThreshold : -Number.MAX_VALUE, t)), x = m(b.zMax, Math.max(x, v(e))))))
                        }); H.forEach(function (b) { var c = b[l], h = c.length; k && b.getRadii(t, x, b); if (0 < y) for (; h--;)if (d(c[h]) && a.dataMin <= c[h] && c[h] <= a.max) { var q = b.radii ? b.radii[h] : 0; g = Math.min((c[h] - f) * A - q, g); e = Math.max((c[h] - f) * A + q, e) } }); H.length && 0 < y && !this.isLog && (e -= b, A *= (b + Math.max(0, g) - Math.min(e, b)) / b, [["min", "userMin", g], ["max", "userMax", e]].forEach(function (e) {
                        "undefined" ===
                            typeof m(a.options[e[0]], a[e[1]]) && (a[e[0]] += e[2] / A)
                        }))
                    }; ""
        }); E(f, "modules/networkgraph/integrations.js", [f["parts/Globals.js"]], function (l) {
        l.networkgraphIntegrations = {
            verlet: {
                attractiveForceFunction: function (a, c) { return (c - a) / a }, repulsiveForceFunction: function (a, c) { return (c - a) / a * (c > a ? 1 : 0) }, barycenter: function () {
                    var a = this.options.gravitationalConstant, c = this.barycenter.xFactor, b = this.barycenter.yFactor; c = (c - (this.box.left + this.box.width) / 2) * a; b = (b - (this.box.top + this.box.height) / 2) * a; this.nodes.forEach(function (a) {
                    a.fixedPosition ||
                        (a.plotX -= c / a.mass / a.degree, a.plotY -= b / a.mass / a.degree)
                    })
                }, repulsive: function (a, c, b) { c = c * this.diffTemperature / a.mass / a.degree; a.fixedPosition || (a.plotX += b.x * c, a.plotY += b.y * c) }, attractive: function (a, c, b) {
                    var l = a.getMass(), f = -b.x * c * this.diffTemperature; c = -b.y * c * this.diffTemperature; a.fromNode.fixedPosition || (a.fromNode.plotX -= f * l.fromNode / a.fromNode.degree, a.fromNode.plotY -= c * l.fromNode / a.fromNode.degree); a.toNode.fixedPosition || (a.toNode.plotX += f * l.toNode / a.toNode.degree, a.toNode.plotY += c * l.toNode /
                        a.toNode.degree)
                }, integrate: function (a, c) { var b = -a.options.friction, l = a.options.maxSpeed, f = (c.plotX + c.dispX - c.prevX) * b; b *= c.plotY + c.dispY - c.prevY; var w = Math.abs, z = w(f) / (f || 1); w = w(b) / (b || 1); f = z * Math.min(l, Math.abs(f)); b = w * Math.min(l, Math.abs(b)); c.prevX = c.plotX + c.dispX; c.prevY = c.plotY + c.dispY; c.plotX += f; c.plotY += b; c.temperature = a.vectorLength({ x: f, y: b }) }, getK: function (a) { return Math.pow(a.box.width * a.box.height / a.nodes.length, .5) }
            }, euler: {
                attractiveForceFunction: function (a, c) { return a * a / c }, repulsiveForceFunction: function (a,
                    c) { return c * c / a }, barycenter: function () { var a = this.options.gravitationalConstant, c = this.barycenter.xFactor, b = this.barycenter.yFactor; this.nodes.forEach(function (f) { if (!f.fixedPosition) { var l = f.getDegree(); l *= 1 + l / 2; f.dispX += (c - f.plotX) * a * l / f.degree; f.dispY += (b - f.plotY) * a * l / f.degree } }) }, repulsive: function (a, c, b, f) { a.dispX += b.x / f * c / a.degree; a.dispY += b.y / f * c / a.degree }, attractive: function (a, c, b, f) {
                        var l = a.getMass(), u = b.x / f * c; c *= b.y / f; a.fromNode.fixedPosition || (a.fromNode.dispX -= u * l.fromNode / a.fromNode.degree,
                            a.fromNode.dispY -= c * l.fromNode / a.fromNode.degree); a.toNode.fixedPosition || (a.toNode.dispX += u * l.toNode / a.toNode.degree, a.toNode.dispY += c * l.toNode / a.toNode.degree)
                    }, integrate: function (a, c) { c.dispX += c.dispX * a.options.friction; c.dispY += c.dispY * a.options.friction; var b = c.temperature = a.vectorLength({ x: c.dispX, y: c.dispY }); 0 !== b && (c.plotX += c.dispX / b * Math.min(Math.abs(c.dispX), a.temperature), c.plotY += c.dispY / b * Math.min(Math.abs(c.dispY), a.temperature)) }, getK: function (a) {
                        return Math.pow(a.box.width * a.box.height /
                            a.nodes.length, .3)
                    }
            }
        }
        }); E(f, "modules/networkgraph/QuadTree.js", [f["parts/Globals.js"], f["parts/Utilities.js"]], function (f, a) {
            a = a.extend; var c = f.QuadTreeNode = function (a) { this.box = a; this.boxSize = Math.min(a.width, a.height); this.nodes = []; this.body = this.isInternal = !1; this.isEmpty = !0 }; a(c.prototype, {
                insert: function (a, f) {
                    this.isInternal ? this.nodes[this.getBoxPosition(a)].insert(a, f - 1) : (this.isEmpty = !1, this.body ? f ? (this.isInternal = !0, this.divideBox(), !0 !== this.body && (this.nodes[this.getBoxPosition(this.body)].insert(this.body,
                        f - 1), this.body = !0), this.nodes[this.getBoxPosition(a)].insert(a, f - 1)) : (f = new c({ top: a.plotX, left: a.plotY, width: .1, height: .1 }), f.body = a, f.isInternal = !1, this.nodes.push(f)) : (this.isInternal = !1, this.body = a))
                }, updateMassAndCenter: function () { var a = 0, c = 0, f = 0; this.isInternal ? (this.nodes.forEach(function (b) { b.isEmpty || (a += b.mass, c += b.plotX * b.mass, f += b.plotY * b.mass) }), c /= a, f /= a) : this.body && (a = this.body.mass, c = this.body.plotX, f = this.body.plotY); this.mass = a; this.plotX = c; this.plotY = f }, divideBox: function () {
                    var a =
                        this.box.width / 2, f = this.box.height / 2; this.nodes[0] = new c({ left: this.box.left, top: this.box.top, width: a, height: f }); this.nodes[1] = new c({ left: this.box.left + a, top: this.box.top, width: a, height: f }); this.nodes[2] = new c({ left: this.box.left + a, top: this.box.top + f, width: a, height: f }); this.nodes[3] = new c({ left: this.box.left, top: this.box.top + f, width: a, height: f })
                }, getBoxPosition: function (a) { var b = a.plotY < this.box.top + this.box.height / 2; return a.plotX < this.box.left + this.box.width / 2 ? b ? 0 : 3 : b ? 1 : 2 }
            }); f = f.QuadTree = function (a,
                f, l, w) { this.box = { left: a, top: f, width: l, height: w }; this.maxDepth = 25; this.root = new c(this.box, "0"); this.root.isInternal = !0; this.root.isRoot = !0; this.root.divideBox() }; a(f.prototype, {
                    insertNodes: function (a) { a.forEach(function (a) { this.root.insert(a, this.maxDepth) }, this) }, visitNodeRecursive: function (a, c, f) {
                        var b; a || (a = this.root); a === this.root && c && (b = c(a)); !1 !== b && (a.nodes.forEach(function (a) { if (a.isInternal) { c && (b = c(a)); if (!1 === b) return; this.visitNodeRecursive(a, c, f) } else a.body && c && c(a.body); f && f(a) }, this),
                            a === this.root && f && f(a))
                    }, calculateMassAndCenter: function () { this.visitNodeRecursive(null, null, function (a) { a.updateMassAndCenter() }) }
                })
        }); E(f, "modules/networkgraph/layouts.js", [f["parts/Globals.js"], f["parts/Utilities.js"]], function (f, a) {
            var c = a.addEvent, b = a.clamp, l = a.defined, v = a.extend, w = a.isFunction, z = a.pick, y = a.setAnimation; a = f.Chart; f.layouts = { "reingold-fruchterman": function () { } }; v(f.layouts["reingold-fruchterman"].prototype, {
                init: function (a) {
                this.options = a; this.nodes = []; this.links = []; this.series =
                    []; this.box = { x: 0, y: 0, width: 0, height: 0 }; this.setInitialRendering(!0); this.integration = f.networkgraphIntegrations[a.integration]; this.attractiveForce = z(a.attractiveForce, this.integration.attractiveForceFunction); this.repulsiveForce = z(a.repulsiveForce, this.integration.repulsiveForceFunction); this.approximation = a.approximation
                }, start: function () {
                    var a = this.series, b = this.options; this.currentStep = 0; this.forces = a[0] && a[0].forces || []; this.initialRendering && (this.initPositions(), a.forEach(function (a) { a.render() }));
                    this.setK(); this.resetSimulation(b); b.enableSimulation && this.step()
                }, step: function () {
                    var a = this, b = this.series, c = this.options; a.currentStep++; "barnes-hut" === a.approximation && (a.createQuadTree(), a.quadTree.calculateMassAndCenter()); a.forces.forEach(function (b) { a[b + "Forces"](a.temperature) }); a.applyLimits(a.temperature); a.temperature = a.coolDown(a.startTemperature, a.diffTemperature, a.currentStep); a.prevSystemTemperature = a.systemTemperature; a.systemTemperature = a.getSystemTemperature(); c.enableSimulation &&
                        (b.forEach(function (a) { a.chart && a.render() }), a.maxIterations-- && isFinite(a.temperature) && !a.isStable() ? (a.simulation && f.win.cancelAnimationFrame(a.simulation), a.simulation = f.win.requestAnimationFrame(function () { a.step() })) : a.simulation = !1)
                }, stop: function () { this.simulation && f.win.cancelAnimationFrame(this.simulation) }, setArea: function (a, b, c, f) { this.box = { left: a, top: b, width: c, height: f } }, setK: function () { this.k = this.options.linkLength || this.integration.getK(this) }, addElementsToCollection: function (a, b) {
                    a.forEach(function (a) {
                    -1 ===
                        b.indexOf(a) && b.push(a)
                    })
                }, removeElementFromCollection: function (a, b) { a = b.indexOf(a); -1 !== a && b.splice(a, 1) }, clear: function () { this.nodes.length = 0; this.links.length = 0; this.series.length = 0; this.resetSimulation() }, resetSimulation: function () { this.forcedStop = !1; this.systemTemperature = 0; this.setMaxIterations(); this.setTemperature(); this.setDiffTemperature() }, setMaxIterations: function (a) { this.maxIterations = z(a, this.options.maxIterations) }, setTemperature: function () { this.temperature = this.startTemperature = Math.sqrt(this.nodes.length) },
                setDiffTemperature: function () { this.diffTemperature = this.startTemperature / (this.options.maxIterations + 1) }, setInitialRendering: function (a) { this.initialRendering = a }, createQuadTree: function () { this.quadTree = new f.QuadTree(this.box.left, this.box.top, this.box.width, this.box.height); this.quadTree.insertNodes(this.nodes) }, initPositions: function () {
                    var a = this.options.initialPositions; w(a) ? (a.call(this), this.nodes.forEach(function (a) {
                    l(a.prevX) || (a.prevX = a.plotX); l(a.prevY) || (a.prevY = a.plotY); a.dispX = 0; a.dispY =
                        0
                    })) : "circle" === a ? this.setCircularPositions() : this.setRandomPositions()
                }, setCircularPositions: function () {
                    function a(b) { b.linksFrom.forEach(function (b) { r[b.toNode.id] || (r[b.toNode.id] = !0, u.push(b.toNode), a(b.toNode)) }) } var b = this.box, c = this.nodes, f = 2 * Math.PI / (c.length + 1), l = c.filter(function (a) { return 0 === a.linksTo.length }), u = [], r = {}, p = this.options.initialPositionRadius; l.forEach(function (b) { u.push(b); a(b) }); u.length ? c.forEach(function (a) { -1 === u.indexOf(a) && u.push(a) }) : u = c; u.forEach(function (a, d) {
                    a.plotX =
                        a.prevX = z(a.plotX, b.width / 2 + p * Math.cos(d * f)); a.plotY = a.prevY = z(a.plotY, b.height / 2 + p * Math.sin(d * f)); a.dispX = 0; a.dispY = 0
                    })
                }, setRandomPositions: function () { function a(a) { a = a * a / Math.PI; return a -= Math.floor(a) } var b = this.box, c = this.nodes, f = c.length + 1; c.forEach(function (d, c) { d.plotX = d.prevX = z(d.plotX, b.width * a(c)); d.plotY = d.prevY = z(d.plotY, b.height * a(f + c)); d.dispX = 0; d.dispY = 0 }) }, force: function (a) { this.integration[a].apply(this, Array.prototype.slice.call(arguments, 1)) }, barycenterForces: function () {
                    this.getBarycenter();
                    this.force("barycenter")
                }, getBarycenter: function () { var a = 0, b = 0, c = 0; this.nodes.forEach(function (d) { b += d.plotX * d.mass; c += d.plotY * d.mass; a += d.mass }); return this.barycenter = { x: b, y: c, xFactor: b / a, yFactor: c / a } }, barnesHutApproximation: function (a, b) {
                    var d = this.getDistXY(a, b), c = this.vectorLength(d); if (a !== b && 0 !== c) if (b.isInternal) if (b.boxSize / c < this.options.theta && 0 !== c) { var f = this.repulsiveForce(c, this.k); this.force("repulsive", a, f * b.mass, d, c); var l = !1 } else l = !0; else f = this.repulsiveForce(c, this.k), this.force("repulsive",
                        a, f * b.mass, d, c); return l
                }, repulsiveForces: function () { var a = this; "barnes-hut" === a.approximation ? a.nodes.forEach(function (b) { a.quadTree.visitNodeRecursive(null, function (d) { return a.barnesHutApproximation(b, d) }) }) : a.nodes.forEach(function (b) { a.nodes.forEach(function (d) { if (b !== d && !b.fixedPosition) { var c = a.getDistXY(b, d); var f = a.vectorLength(c); if (0 !== f) { var l = a.repulsiveForce(f, a.k); a.force("repulsive", b, l * d.mass, c, f) } } }) }) }, attractiveForces: function () {
                    var a = this, b, c, f; a.links.forEach(function (d) {
                    d.fromNode &&
                        d.toNode && (b = a.getDistXY(d.fromNode, d.toNode), c = a.vectorLength(b), 0 !== c && (f = a.attractiveForce(c, a.k), a.force("attractive", d, f, b, c)))
                    })
                }, applyLimits: function () { var a = this; a.nodes.forEach(function (b) { b.fixedPosition || (a.integration.integrate(a, b), a.applyLimitBox(b, a.box), b.dispX = 0, b.dispY = 0) }) }, applyLimitBox: function (a, c) { var d = a.radius; a.plotX = b(a.plotX, c.left + d, c.width - d); a.plotY = b(a.plotY, c.top + d, c.height - d) }, coolDown: function (a, b, c) { return a - b * c }, isStable: function () {
                    return .00001 > Math.abs(this.systemTemperature -
                        this.prevSystemTemperature) || 0 >= this.temperature
                }, getSystemTemperature: function () { return this.nodes.reduce(function (a, b) { return a + b.temperature }, 0) }, vectorLength: function (a) { return Math.sqrt(a.x * a.x + a.y * a.y) }, getDistR: function (a, b) { a = this.getDistXY(a, b); return this.vectorLength(a) }, getDistXY: function (a, b) { var c = a.plotX - b.plotX; a = a.plotY - b.plotY; return { x: c, y: a, absX: Math.abs(c), absY: Math.abs(a) } }
            }); c(a, "predraw", function () { this.graphLayoutsLookup && this.graphLayoutsLookup.forEach(function (a) { a.stop() }) });
            c(a, "render", function () { function a(a) { a.maxIterations-- && isFinite(a.temperature) && !a.isStable() && !a.options.enableSimulation && (a.beforeStep && a.beforeStep(), a.step(), c = !1, b = !0) } var b = !1; if (this.graphLayoutsLookup) { y(!1, this); for (this.graphLayoutsLookup.forEach(function (a) { a.start() }); !c;) { var c = !0; this.graphLayoutsLookup.forEach(a) } b && this.series.forEach(function (a) { a && a.layout && a.render() }) } })
        }); E(f, "modules/networkgraph/draggable-nodes.js", [f["parts/Globals.js"], f["parts/Utilities.js"]], function (f,
            a) {
                var c = a.addEvent; a = f.Chart; f.dragNodesMixin = {
                    onMouseDown: function (a, c) { c = this.chart.pointer.normalize(c); a.fixedPosition = { chartX: c.chartX, chartY: c.chartY, plotX: a.plotX, plotY: a.plotY }; a.inDragMode = !0 }, onMouseMove: function (a, c) {
                        if (a.fixedPosition && a.inDragMode) {
                            var b = this.chart, f = b.pointer.normalize(c); c = a.fixedPosition.chartX - f.chartX; f = a.fixedPosition.chartY - f.chartY; if (5 < Math.abs(c) || 5 < Math.abs(f)) c = a.fixedPosition.plotX - c, f = a.fixedPosition.plotY - f, b.isInsidePlot(c, f) && (a.plotX = c, a.plotY = f, a.hasDragged =
                                !0, this.redrawHalo(a), this.layout.simulation ? this.layout.resetSimulation() : (this.layout.setInitialRendering(!1), this.layout.enableSimulation ? this.layout.start() : this.layout.setMaxIterations(1), this.chart.redraw(), this.layout.setInitialRendering(!0)))
                        }
                    }, onMouseUp: function (a, c) { a.fixedPosition && a.hasDragged && (this.layout.enableSimulation ? this.layout.start() : this.chart.redraw(), a.inDragMode = a.hasDragged = !1, this.options.fixedDraggable || delete a.fixedPosition) }, redrawHalo: function (a) { a && this.halo && this.halo.attr({ d: a.haloPath(this.options.states.hover.halo.size) }) }
                };
            c(a, "load", function () { var a = this, f, l, w; a.container && (f = c(a.container, "mousedown", function (b) { var f = a.hoverPoint; f && f.series && f.series.hasDraggableNodes && f.series.options.draggable && (f.series.onMouseDown(f, b), l = c(a.container, "mousemove", function (a) { return f && f.series && f.series.onMouseMove(f, a) }), w = c(a.container.ownerDocument, "mouseup", function (a) { l(); w(); return f && f.series && f.series.onMouseUp(f, a) })) })); c(a, "destroy", function () { f() }) })
        }); E(f, "parts-more/PackedBubbleSeries.js", [f["parts/Globals.js"],
        f["parts/Color.js"], f["parts/Point.js"], f["parts/Utilities.js"]], function (f, a, c, b) {
            var l = a.parse, v = b.addEvent, w = b.clamp, z = b.defined, y = b.extend; a = b.extendClass; var d = b.fireEvent, m = b.isArray, n = b.isNumber, t = b.merge, x = b.pick; b = b.seriesType; var A = f.Series, r = f.Chart, p = f.layouts["reingold-fruchterman"], h = f.seriesTypes.bubble.prototype.pointClass, g = f.dragNodesMixin; f.networkgraphIntegrations.packedbubble = {
                repulsiveForceFunction: function (a, b, c, d) { return Math.min(a, (c.marker.radius + d.marker.radius) / 2) }, barycenter: function () {
                    var a =
                        this, b = a.options.gravitationalConstant, c = a.box, d = a.nodes, f, g; d.forEach(function (e) { a.options.splitSeries && !e.isParentNode ? (f = e.series.parentNode.plotX, g = e.series.parentNode.plotY) : (f = c.width / 2, g = c.height / 2); e.fixedPosition || (e.plotX -= (e.plotX - f) * b / (e.mass * Math.sqrt(d.length)), e.plotY -= (e.plotY - g) * b / (e.mass * Math.sqrt(d.length))) })
                }, repulsive: function (a, b, c, d) {
                    var e = b * this.diffTemperature / a.mass / a.degree; b = c.x * e; c = c.y * e; a.fixedPosition || (a.plotX += b, a.plotY += c); d.fixedPosition || (d.plotX -= b, d.plotY -=
                        c)
                }, integrate: f.networkgraphIntegrations.verlet.integrate, getK: f.noop
            }; f.layouts.packedbubble = a(p, {
                beforeStep: function () { this.options.marker && this.series.forEach(function (a) { a && a.calculateParentRadius() }) }, setCircularPositions: function () {
                    var a = this, b = a.box, c = a.nodes, d = 2 * Math.PI / (c.length + 1), f, g, h = a.options.initialPositionRadius; c.forEach(function (e, c) {
                        a.options.splitSeries && !e.isParentNode ? (f = e.series.parentNode.plotX, g = e.series.parentNode.plotY) : (f = b.width / 2, g = b.height / 2); e.plotX = e.prevX = x(e.plotX,
                            f + h * Math.cos(e.index || c * d)); e.plotY = e.prevY = x(e.plotY, g + h * Math.sin(e.index || c * d)); e.dispX = 0; e.dispY = 0
                    })
                }, repulsiveForces: function () {
                    var a = this, b, c, d, f = a.options.bubblePadding; a.nodes.forEach(function (e) {
                    e.degree = e.mass; e.neighbours = 0; a.nodes.forEach(function (k) {
                        b = 0; e === k || e.fixedPosition || !a.options.seriesInteraction && e.series !== k.series || (d = a.getDistXY(e, k), c = a.vectorLength(d) - (e.marker.radius + k.marker.radius + f), 0 > c && (e.degree += .01, e.neighbours++ , b = a.repulsiveForce(-c / Math.sqrt(e.neighbours), a.k,
                            e, k)), a.force("repulsive", e, b * k.mass, d, k, c))
                    })
                    })
                }, applyLimitBox: function (a) { if (this.options.splitSeries && !a.isParentNode && this.options.parentNodeLimit) { var e = this.getDistXY(a, a.series.parentNode); var b = a.series.parentNodeRadius - a.marker.radius - this.vectorLength(e); 0 > b && b > -2 * a.marker.radius && (a.plotX -= .01 * e.x, a.plotY -= .01 * e.y) } p.prototype.applyLimitBox.apply(this, arguments) }, isStable: function () {
                    return .00001 > Math.abs(this.systemTemperature - this.prevSystemTemperature) || 0 >= this.temperature || 0 < this.systemTemperature &&
                        .02 > this.systemTemperature / this.nodes.length && this.enableSimulation
                }
            }); b("packedbubble", "bubble", {
                minSize: "10%", maxSize: "50%", sizeBy: "area", zoneAxis: "y", tooltip: { pointFormat: "Value: {point.value}" }, draggable: !0, useSimulation: !0, dataLabels: { formatter: function () { return this.point.value }, parentNodeFormatter: function () { return this.name }, parentNodeTextPath: { enabled: !0 }, padding: 0 }, layoutAlgorithm: {
                    initialPositions: "circle", initialPositionRadius: 20, bubblePadding: 5, parentNodeLimit: !1, seriesInteraction: !0,
                    dragBetweenSeries: !1, parentNodeOptions: { maxIterations: 400, gravitationalConstant: .03, maxSpeed: 50, initialPositionRadius: 100, seriesInteraction: !0, marker: { fillColor: null, fillOpacity: 1, lineWidth: 1, lineColor: null, symbol: "circle" } }, enableSimulation: !0, type: "packedbubble", integration: "packedbubble", maxIterations: 1E3, splitSeries: !1, maxSpeed: 5, gravitationalConstant: .01, friction: -.981
                }
            }, {
                hasDraggableNodes: !0, forces: ["barycenter", "repulsive"], pointArrayMap: ["value"], pointValKey: "value", isCartesian: !1, requireSorting: !1,
                    directTouch: !0, axisTypes: [], noSharedTooltip: !0, searchPoint: f.noop, accumulateAllPoints: function (a) { var e = a.chart, b = [], c, d; for (c = 0; c < e.series.length; c++)if (a = e.series[c], a.visible || !e.options.chart.ignoreHiddenSeries) for (d = 0; d < a.yData.length; d++)b.push([null, null, a.yData[d], a.index, d, { id: d, marker: { radius: 0 } }]); return b }, init: function () { A.prototype.init.apply(this, arguments); v(this, "updatedData", function () { this.chart.series.forEach(function (a) { a.type === this.type && (a.isDirty = !0) }, this) }); return this },
                    render: function () { var a = []; A.prototype.render.apply(this, arguments); this.options.dataLabels.allowOverlap || (this.data.forEach(function (e) { m(e.dataLabels) && e.dataLabels.forEach(function (e) { a.push(e) }) }), this.options.useSimulation && this.chart.hideOverlappingLabels(a)) }, setVisible: function () {
                        var a = this; A.prototype.setVisible.apply(a, arguments); a.parentNodeLayout && a.graph ? a.visible ? (a.graph.show(), a.parentNode.dataLabel && a.parentNode.dataLabel.show()) : (a.graph.hide(), a.parentNodeLayout.removeElementFromCollection(a.parentNode,
                            a.parentNodeLayout.nodes), a.parentNode.dataLabel && a.parentNode.dataLabel.hide()) : a.layout && (a.visible ? a.layout.addElementsToCollection(a.points, a.layout.nodes) : a.points.forEach(function (e) { a.layout.removeElementFromCollection(e, a.layout.nodes) }))
                    }, drawDataLabels: function () {
                        var a = this.options.dataLabels.textPath, b = this.points; A.prototype.drawDataLabels.apply(this, arguments); this.parentNode && (this.parentNode.formatPrefix = "parentNode", this.points = [this.parentNode], this.options.dataLabels.textPath = this.options.dataLabels.parentNodeTextPath,
                            A.prototype.drawDataLabels.apply(this, arguments), this.points = b, this.options.dataLabels.textPath = a)
                    }, seriesBox: function () { var a = this.chart, b = Math.max, c = Math.min, d, f = [a.plotLeft, a.plotLeft + a.plotWidth, a.plotTop, a.plotTop + a.plotHeight]; this.data.forEach(function (a) { z(a.plotX) && z(a.plotY) && a.marker.radius && (d = a.marker.radius, f[0] = c(f[0], a.plotX - d), f[1] = b(f[1], a.plotX + d), f[2] = c(f[2], a.plotY - d), f[3] = b(f[3], a.plotY + d)) }); return n(f.width / f.height) ? f : null }, calculateParentRadius: function () {
                        var a = this.seriesBox();
                        this.parentNodeRadius = w(Math.sqrt(2 * this.parentNodeMass / Math.PI) + 20, 20, a ? Math.max(Math.sqrt(Math.pow(a.width, 2) + Math.pow(a.height, 2)) / 2 + 20, 20) : Math.sqrt(2 * this.parentNodeMass / Math.PI) + 20); this.parentNode && (this.parentNode.marker.radius = this.parentNode.radius = this.parentNodeRadius)
                    }, drawGraph: function () {
                        if (this.layout && this.layout.options.splitSeries) {
                            var a = this.chart, b = this.layout.options.parentNodeOptions.marker; b = {
                                fill: b.fillColor || l(this.color).brighten(.4).get(), opacity: b.fillOpacity, stroke: b.lineColor ||
                                    this.color, "stroke-width": b.lineWidth
                            }; var c = this.visible ? "inherit" : "hidden"; this.parentNodesGroup || (this.parentNodesGroup = this.plotGroup("parentNodesGroup", "parentNode", c, .1, a.seriesGroup), this.group.attr({ zIndex: 2 })); this.calculateParentRadius(); c = t({ x: this.parentNode.plotX - this.parentNodeRadius, y: this.parentNode.plotY - this.parentNodeRadius, width: 2 * this.parentNodeRadius, height: 2 * this.parentNodeRadius }, b); this.parentNode.graphic || (this.graph = this.parentNode.graphic = a.renderer.symbol(b.symbol).add(this.parentNodesGroup));
                            this.parentNode.graphic.attr(c)
                        }
                    }, createParentNodes: function () {
                        var a = this, b = a.chart, c = a.parentNodeLayout, d, f = a.parentNode; a.parentNodeMass = 0; a.points.forEach(function (e) { a.parentNodeMass += Math.PI * Math.pow(e.marker.radius, 2) }); a.calculateParentRadius(); c.nodes.forEach(function (e) { e.seriesIndex === a.index && (d = !0) }); c.setArea(0, 0, b.plotWidth, b.plotHeight); d || (f || (f = (new h).init(this, {
                            mass: a.parentNodeRadius / 2, marker: { radius: a.parentNodeRadius }, dataLabels: { inside: !1 }, dataLabelOnNull: !0, degree: a.parentNodeRadius,
                            isParentNode: !0, seriesIndex: a.index
                        })), a.parentNode && (f.plotX = a.parentNode.plotX, f.plotY = a.parentNode.plotY), a.parentNode = f, c.addElementsToCollection([a], c.series), c.addElementsToCollection([f], c.nodes))
                    }, addSeriesLayout: function () {
                        var a = this.options.layoutAlgorithm, b = this.chart.graphLayoutsStorage, c = this.chart.graphLayoutsLookup, d = t(a, a.parentNodeOptions, { enableSimulation: this.layout.options.enableSimulation }); var g = b[a.type + "-series"]; g || (b[a.type + "-series"] = g = new f.layouts[a.type], g.init(d), c.splice(g.index,
                            0, g)); this.parentNodeLayout = g; this.createParentNodes()
                    }, addLayout: function () {
                        var a = this.options.layoutAlgorithm, b = this.chart.graphLayoutsStorage, c = this.chart.graphLayoutsLookup, d = this.chart.options.chart; b || (this.chart.graphLayoutsStorage = b = {}, this.chart.graphLayoutsLookup = c = []); var g = b[a.type]; g || (a.enableSimulation = z(d.forExport) ? !d.forExport : a.enableSimulation, b[a.type] = g = new f.layouts[a.type], g.init(a), c.splice(g.index, 0, g)); this.layout = g; this.points.forEach(function (a) {
                        a.mass = 2; a.degree = 1; a.collisionNmb =
                            1
                        }); g.setArea(0, 0, this.chart.plotWidth, this.chart.plotHeight); g.addElementsToCollection([this], g.series); g.addElementsToCollection(this.points, g.nodes)
                    }, deferLayout: function () { var a = this.options.layoutAlgorithm; this.visible && (this.addLayout(), a.splitSeries && this.addSeriesLayout()) }, translate: function () {
                        var a = this.chart, b = this.data, c = this.index, f, g = this.options.useSimulation; this.processedXData = this.xData; this.generatePoints(); z(a.allDataPoints) || (a.allDataPoints = this.accumulateAllPoints(this), this.getPointRadius());
                        if (g) var h = a.allDataPoints; else h = this.placeBubbles(a.allDataPoints), this.options.draggable = !1; for (f = 0; f < h.length; f++)if (h[f][3] === c) { var p = b[h[f][4]]; var r = h[f][2]; g || (p.plotX = h[f][0] - a.plotLeft + a.diffX, p.plotY = h[f][1] - a.plotTop + a.diffY); p.marker = y(p.marker, { radius: r, width: 2 * r, height: 2 * r }); p.radius = r } g && this.deferLayout(); d(this, "afterTranslate")
                    }, checkOverlap: function (a, b) { var e = a[0] - b[0], c = a[1] - b[1]; return -.001 > Math.sqrt(e * e + c * c) - Math.abs(a[2] + b[2]) }, positionBubble: function (a, b, c) {
                        var e = Math.sqrt,
                        d = Math.asin, f = Math.acos, g = Math.pow, k = Math.abs; e = e(g(a[0] - b[0], 2) + g(a[1] - b[1], 2)); f = f((g(e, 2) + g(c[2] + b[2], 2) - g(c[2] + a[2], 2)) / (2 * (c[2] + b[2]) * e)); d = d(k(a[0] - b[0]) / e); a = (0 > a[1] - b[1] ? 0 : Math.PI) + f + d * (0 > (a[0] - b[0]) * (a[1] - b[1]) ? 1 : -1); return [b[0] + (b[2] + c[2]) * Math.sin(a), b[1] - (b[2] + c[2]) * Math.cos(a), c[2], c[3], c[4]]
                    }, placeBubbles: function (a) {
                        var b = this.checkOverlap, e = this.positionBubble, c = [], d = 1, f = 0, g = 0; var h = []; var p; a = a.sort(function (a, b) { return b[2] - a[2] }); if (a.length) {
                            c.push([[0, 0, a[0][2], a[0][3], a[0][4]]]);
                            if (1 < a.length) for (c.push([[0, 0 - a[1][2] - a[0][2], a[1][2], a[1][3], a[1][4]]]), p = 2; p < a.length; p++)a[p][2] = a[p][2] || 1, h = e(c[d][f], c[d - 1][g], a[p]), b(h, c[d][0]) ? (c.push([]), g = 0, c[d + 1].push(e(c[d][f], c[d][0], a[p])), d++ , f = 0) : 1 < d && c[d - 1][g + 1] && b(h, c[d - 1][g + 1]) ? (g++ , c[d].push(e(c[d][f], c[d - 1][g], a[p])), f++) : (f++ , c[d].push(h)); this.chart.stages = c; this.chart.rawPositions = [].concat.apply([], c); this.resizeRadius(); h = this.chart.rawPositions
                        } return h
                    }, resizeRadius: function () {
                        var a = this.chart, b = a.rawPositions, c = Math.min,
                        d = Math.max, f = a.plotLeft, g = a.plotTop, h = a.plotHeight, p = a.plotWidth, r, l, m; var n = r = Number.POSITIVE_INFINITY; var t = l = Number.NEGATIVE_INFINITY; for (m = 0; m < b.length; m++) { var u = b[m][2]; n = c(n, b[m][0] - u); t = d(t, b[m][0] + u); r = c(r, b[m][1] - u); l = d(l, b[m][1] + u) } m = [t - n, l - r]; c = c.apply([], [(p - f) / m[0], (h - g) / m[1]]); if (1e-10 < Math.abs(c - 1)) { for (m = 0; m < b.length; m++)b[m][2] *= c; this.placeBubbles(b) } else a.diffY = h / 2 + g - r - (l - r) / 2, a.diffX = p / 2 + f - n - (t - n) / 2
                    }, calculateZExtremes: function () {
                        var a = this.options.zMin, b = this.options.zMax, c =
                            Infinity, d = -Infinity; if (a && b) return [a, b]; this.chart.series.forEach(function (a) { a.yData.forEach(function (a) { z(a) && (a > d && (d = a), a < c && (c = a)) }) }); a = x(a, c); b = x(b, d); return [a, b]
                    }, getPointRadius: function () {
                        var a = this, b = a.chart, c = a.options, d = c.useSimulation, f = Math.min(b.plotWidth, b.plotHeight), g = {}, h = [], p = b.allDataPoints, r, l, m, n;["minSize", "maxSize"].forEach(function (a) { var b = parseInt(c[a], 10), e = /%$/.test(c[a]); g[a] = e ? f * b / 100 : b * Math.sqrt(p.length) }); b.minRadius = r = g.minSize / Math.sqrt(p.length); b.maxRadius =
                            l = g.maxSize / Math.sqrt(p.length); var t = d ? a.calculateZExtremes() : [r, l]; (p || []).forEach(function (b, e) { m = d ? w(b[2], t[0], t[1]) : b[2]; n = a.getRadius(t[0], t[1], r, l, m); 0 === n && (n = null); p[e][2] = n; h.push(n) }); a.radii = h
                    }, redrawHalo: g.redrawHalo, onMouseDown: g.onMouseDown, onMouseMove: g.onMouseMove, onMouseUp: function (a) {
                        if (a.fixedPosition && !a.removed) {
                            var b, c, e = this.layout, d = this.parentNodeLayout; d && e.options.dragBetweenSeries && d.nodes.forEach(function (d) {
                            a && a.marker && d !== a.series.parentNode && (b = e.getDistXY(a, d), c =
                                e.vectorLength(b) - d.marker.radius - a.marker.radius, 0 > c && (d.series.addPoint(t(a.options, { plotX: a.plotX, plotY: a.plotY }), !1), e.removeElementFromCollection(a, e.nodes), a.remove()))
                            }); g.onMouseUp.apply(this, arguments)
                        }
                    }, destroy: function () {
                        this.chart.graphLayoutsLookup && this.chart.graphLayoutsLookup.forEach(function (a) { a.removeElementFromCollection(this, a.series) }, this); this.parentNode && (this.parentNodeLayout.removeElementFromCollection(this.parentNode, this.parentNodeLayout.nodes), this.parentNode.dataLabel &&
                            (this.parentNode.dataLabel = this.parentNode.dataLabel.destroy())); f.Series.prototype.destroy.apply(this, arguments)
                    }, alignDataLabel: f.Series.prototype.alignDataLabel
                }, { destroy: function () { this.series.layout && this.series.layout.removeElementFromCollection(this, this.series.layout.nodes); return c.prototype.destroy.apply(this, arguments) } }); v(r, "beforeRedraw", function () { this.allDataPoints && delete this.allDataPoints }); ""
        }); E(f, "parts-more/Polar.js", [f["parts/Globals.js"], f["parts/Utilities.js"], f["parts-more/Pane.js"]],
            function (f, a, c) {
                var b = a.addEvent, l = a.defined, v = a.find, w = a.pick, z = a.splat, y = a.uniqueKey, d = a.wrap, m = f.Series, n = f.seriesTypes, t = m.prototype, x = f.Pointer.prototype; t.searchPointByAngle = function (a) { var b = this.chart, c = this.xAxis.pane.center; return this.searchKDTree({ clientX: 180 + -180 / Math.PI * Math.atan2(a.chartX - c[0] - b.plotLeft, a.chartY - c[1] - b.plotTop) }) }; t.getConnectors = function (a, b, c, d) {
                    var e = d ? 1 : 0; var f = 0 <= b && b <= a.length - 1 ? b : 0 > b ? a.length - 1 + b : 0; b = 0 > f - 1 ? a.length - (1 + e) : f - 1; e = f + 1 > a.length - 1 ? e : f + 1; var g = a[b];
                    e = a[e]; var h = g.plotX; g = g.plotY; var p = e.plotX; var r = e.plotY; e = a[f].plotX; f = a[f].plotY; h = (1.5 * e + h) / 2.5; g = (1.5 * f + g) / 2.5; p = (1.5 * e + p) / 2.5; var l = (1.5 * f + r) / 2.5; r = Math.sqrt(Math.pow(h - e, 2) + Math.pow(g - f, 2)); var m = Math.sqrt(Math.pow(p - e, 2) + Math.pow(l - f, 2)); h = Math.atan2(g - f, h - e); l = Math.PI / 2 + (h + Math.atan2(l - f, p - e)) / 2; Math.abs(h - l) > Math.PI / 2 && (l -= Math.PI); h = e + Math.cos(l) * r; g = f + Math.sin(l) * r; p = e + Math.cos(Math.PI + l) * m; l = f + Math.sin(Math.PI + l) * m; e = { rightContX: p, rightContY: l, leftContX: h, leftContY: g, plotX: e, plotY: f };
                    c && (e.prevPointCont = this.getConnectors(a, b, !1, d)); return e
                }; t.toXY = function (a) {
                    var b = this.chart, c = this.xAxis; var d = this.yAxis; var e = a.plotX, f = a.plotY, l = a.series, r = b.inverted, m = a.y, n = r ? e : d.len - f; r && l && !l.isRadialBar && (a.plotY = f = "number" === typeof m ? d.translate(m) || 0 : 0); a.rectPlotX = e; a.rectPlotY = f; d.center && (n += d.center[3] / 2); d = r ? d.postTranslate(f, n) : c.postTranslate(e, n); a.plotX = a.polarPlotX = d.x - b.plotLeft; a.plotY = a.polarPlotY = d.y - b.plotTop; this.kdByAngle ? (b = (e / Math.PI * 180 + c.pane.options.startAngle) %
                        360, 0 > b && (b += 360), a.clientX = b) : a.clientX = a.plotX
                }; n.spline && (d(n.spline.prototype, "getPointSpline", function (a, b, c, d) { this.chart.polar ? d ? (a = this.getConnectors(b, d, !0, this.connectEnds), a = ["C", a.prevPointCont.rightContX, a.prevPointCont.rightContY, a.leftContX, a.leftContY, a.plotX, a.plotY]) : a = ["M", c.plotX, c.plotY] : a = a.call(this, b, c, d); return a }), n.areasplinerange && (n.areasplinerange.prototype.getPointSpline = n.spline.prototype.getPointSpline)); b(m, "afterTranslate", function () {
                    var a = this.chart; if (a.polar &&
                        this.xAxis) {
                            (this.kdByAngle = a.tooltip && a.tooltip.shared) ? this.searchPoint = this.searchPointByAngle : this.options.findNearestPointBy = "xy"; if (!this.preventPostTranslate) for (var c = this.points, d = c.length; d--;)this.toXY(c[d]), !a.hasParallelCoordinates && !this.yAxis.reversed && c[d].y < this.yAxis.min && (c[d].isNull = !0); this.hasClipCircleSetter || (this.hasClipCircleSetter = !!this.eventsToUnbind.push(b(this, "afterRender", function () {
                                if (a.polar) {
                                    var b = this.yAxis.pane.center; this.clipCircle ? this.clipCircle.animate({
                                        x: b[0],
                                        y: b[1], r: b[2] / 2, innerR: b[3] / 2
                                    }) : this.clipCircle = a.renderer.clipCircle(b[0], b[1], b[2] / 2, b[3] / 2); this.group.clip(this.clipCircle); this.setClip = f.noop
                                }
                            })))
                    }
                }, { order: 2 }); d(t, "getGraphPath", function (a, b) {
                    var c = this, d; if (this.chart.polar) { b = b || this.points; for (d = 0; d < b.length; d++)if (!b[d].isNull) { var e = d; break } if (!1 !== this.options.connectEnds && "undefined" !== typeof e) { this.connectEnds = !0; b.splice(b.length, 0, b[e]); var f = !0 } b.forEach(function (a) { "undefined" === typeof a.polarPlotY && c.toXY(a) }) } d = a.apply(this,
                        [].slice.call(arguments, 1)); f && b.pop(); return d
                }); var A = function (a, b) {
                    var c = this, d = this.chart, e = this.options.animation, k = this.group, p = this.markerGroup, l = this.xAxis.center, m = d.plotLeft, r = d.plotTop, n, t, u, v; if (d.polar) if (c.isRadialBar) b || (c.startAngleRad = w(c.translatedThreshold, c.xAxis.startAngleRad), f.seriesTypes.pie.prototype.animate.call(c, b)); else {
                        if (d.renderer.isSVG) if (e = f.animObject(e), c.is("column")) {
                            if (!b) {
                                var x = l[3] / 2; c.points.forEach(function (a) {
                                    n = a.graphic; u = (t = a.shapeArgs) && t.r; v = t && t.innerR;
                                    n && t && (n.attr({ r: x, innerR: x }), n.animate({ r: u, innerR: v }, c.options.animation))
                                })
                            }
                        } else b ? (a = { translateX: l[0] + m, translateY: l[1] + r, scaleX: .001, scaleY: .001 }, k.attr(a), p && p.attr(a)) : (a = { translateX: m, translateY: r, scaleX: 1, scaleY: 1 }, k.animate(a, e), p && p.animate(a, e))
                    } else a.call(this, b)
                }; d(t, "animate", A); n.column && (m = n.arearange.prototype, n = n.column.prototype, n.polarArc = function (a, b, c, d) {
                    var e = this.xAxis.center, f = this.yAxis.len, g = e[3] / 2; b = f - b + g; a = f - w(a, f) + g; this.yAxis.reversed && (0 > b && (b = g), 0 > a && (a = g)); return {
                        x: e[0],
                        y: e[1], r: b, innerR: a, start: c, end: d
                    }
                }, d(n, "animate", A), d(n, "translate", function (b) {
                    var c = this.options, d = c.stacking, g = this.chart, e = this.xAxis, k = this.yAxis, m = k.reversed, r = k.center, n = e.startAngleRad, t = e.endAngleRad - n; this.preventPostTranslate = !0; b.call(this); if (e.isRadial) {
                        b = this.points; e = b.length; var u = k.translate(k.min); var v = k.translate(k.max); c = c.threshold || 0; if (g.inverted && f.isNumber(c)) { var w = k.translate(c); l(w) && (0 > w ? w = 0 : w > t && (w = t), this.translatedThreshold = w + n) } for (; e--;) {
                            c = b[e]; var x = c.barX; var y =
                                c.x; var A = c.y; c.shapeType = "arc"; if (g.inverted) {
                                c.plotY = k.translate(A); if (d) { if (A = k.stacks[(0 > A ? "-" : "") + this.stackKey], this.visible && A && A[y] && !c.isNull) { var z = A[y].points[this.getStackIndicator(void 0, y, this.index).key]; var D = k.translate(z[0]); z = k.translate(z[1]); l(D) && (D = a.clamp(D, 0, t)) } } else D = w, z = c.plotY; D > z && (z = [D, D = z][0]); if (!m) if (D < u) D = u; else if (z > v) z = v; else { if (z < u || D > v) D = z = 0 } else if (z > u) z = u; else if (D < v) D = v; else if (D > u || z < v) D = z = t; k.min > k.max && (D = z = m ? t : 0); D += n; z += n; r && (c.barX = x += r[3] / 2); y = Math.max(x,
                                    0); A = Math.max(x + c.pointWidth, 0); c.shapeArgs = { x: r && r[0], y: r && r[1], r: A, innerR: y, start: D, end: z }; c.opacity = D === z ? 0 : void 0; c.plotY = (l(this.translatedThreshold) && (D < this.translatedThreshold ? D : z)) - n
                                } else D = x + n, c.shapeArgs = this.polarArc(c.yBottom, c.plotY, D, D + c.pointWidth); this.toXY(c); g.inverted ? (x = k.postTranslate(c.rectPlotY, x + c.pointWidth / 2), c.tooltipPos = [x.x - g.plotLeft, x.y - g.plotTop]) : c.tooltipPos = [c.plotX, c.plotY]; r && (c.ttBelow = c.plotY > r[1])
                        }
                    }
                }), n.findAlignments = function (a, b) {
                null === b.align && (b.align =
                    20 < a && 160 > a ? "left" : 200 < a && 340 > a ? "right" : "center"); null === b.verticalAlign && (b.verticalAlign = 45 > a || 315 < a ? "bottom" : 135 < a && 225 > a ? "top" : "middle"); return b
                }, m && (m.findAlignments = n.findAlignments), d(n, "alignDataLabel", function (a, b, c, d, e, f) {
                    var g = this.chart, h = w(d.inside, !!this.options.stacking); g.polar ? (a = b.rectPlotX / Math.PI * 180, g.inverted ? (this.forceDL = g.isInsidePlot(b.plotX, Math.round(b.plotY), !1), h && b.shapeArgs ? (e = b.shapeArgs, e = this.yAxis.postTranslate((e.start + e.end) / 2 - this.xAxis.startAngleRad, b.barX +
                        b.pointWidth / 2), e = { x: e.x - g.plotLeft, y: e.y - g.plotTop }) : b.tooltipPos && (e = { x: b.tooltipPos[0], y: b.tooltipPos[1] }), d.align = w(d.align, "center"), d.verticalAlign = w(d.verticalAlign, "middle")) : this.findAlignments && (d = this.findAlignments(a, d)), t.alignDataLabel.call(this, b, c, d, e, f), this.isRadialBar && b.shapeArgs && b.shapeArgs.start === b.shapeArgs.end && c.hide(!0)) : a.call(this, b, c, d, e, f)
                })); d(x, "getCoordinates", function (a, b) {
                    var c = this.chart, d = { xAxis: [], yAxis: [] }; c.polar ? c.axes.forEach(function (a) {
                        var e = a.isXAxis,
                        f = a.center; if ("colorAxis" !== a.coll) { var g = b.chartX - f[0] - c.plotLeft; f = b.chartY - f[1] - c.plotTop; d[e ? "xAxis" : "yAxis"].push({ axis: a, value: a.translate(e ? Math.PI - Math.atan2(g, f) : Math.sqrt(Math.pow(g, 2) + Math.pow(f, 2)), !0) }) }
                    }) : d = a.call(this, b); return d
                }); f.SVGRenderer.prototype.clipCircle = function (a, b, c, d) { var e = y(), f = this.createElement("clipPath").attr({ id: e }).add(this.defs); a = d ? this.arc(a, b, c, d, 0, 2 * Math.PI).add(f) : this.circle(a, b, c).add(f); a.id = e; a.clipPath = f; return a }; b(f.Chart, "getAxes", function () {
                this.pane ||
                    (this.pane = []); z(this.options.pane).forEach(function (a) { new c(a, this) }, this)
                }); b(f.Chart, "afterDrawChartBox", function () { this.pane.forEach(function (a) { a.render() }) }); b(f.Series, "afterInit", function () { var a = this.chart; a.inverted && a.polar && (this.isRadialSeries = !0, this.is("column") && (this.isRadialBar = !0)) }); d(f.Chart.prototype, "get", function (a, b) { return v(this.pane, function (a) { return a.options.id === b }) || a.call(this, b) })
            }); E(f, "masters/highcharts-more.src.js", [], function () { })
});
//# sourceMappingURL=highcharts-more.js.map