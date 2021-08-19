﻿/*
 Highcharts JS v8.2.2 (2020-10-22)

 Client side exporting module

 (c) 2015-2019 Torstein Honsi / Oystein Moseng

 License: www.highcharts.com/license
*/
(function (a) { "object" === typeof module && module.exports ? (a["default"] = a, module.exports = a) : "function" === typeof define && define.amd ? define("highcharts/modules/offline-exporting", ["highcharts", "highcharts/modules/exporting"], function (f) { a(f); a.Highcharts = f; return a }) : a("undefined" !== typeof Highcharts ? Highcharts : void 0) })(function (a) {
    function f(a, b, m, e) { a.hasOwnProperty(b) || (a[b] = e.apply(null, m)) } a = a ? a._modules : {}; f(a, "Extensions/DownloadURL.js", [a["Core/Globals.js"]], function (a) {
        var b = a.win, m = b.navigator,
            e = b.document, f = b.URL || b.webkitURL || b, u = /Edge\/\d+/.test(m.userAgent), D = a.dataURLtoBlob = function (a) { if ((a = a.replace(/filename=.*;/, "").match(/data:([^;]*)(;base64)?,([0-9A-Za-z+/]+)/)) && 3 < a.length && b.atob && b.ArrayBuffer && b.Uint8Array && b.Blob && f.createObjectURL) { var x = b.atob(a[3]), d = new b.ArrayBuffer(x.length); d = new b.Uint8Array(d); for (var k = 0; k < d.length; ++k)d[k] = x.charCodeAt(k); a = new b.Blob([d], { type: a[1] }); return f.createObjectURL(a) } }; a = a.downloadURL = function (a, f) {
                var d = e.createElement("a"); if ("string" ===
                    typeof a || a instanceof String || !m.msSaveOrOpenBlob) { a = "" + a; if (u || 2E6 < a.length) if (a = D(a) || "", !a) throw Error("Failed to convert to blob"); if ("undefined" !== typeof d.download) d.href = a, d.download = f, e.body.appendChild(d), d.click(), e.body.removeChild(d); else try { var k = b.open(a, "chart"); if ("undefined" === typeof k || null === k) throw Error("Failed to open window"); } catch (E) { b.location.href = a } } else m.msSaveOrOpenBlob(a, f)
            }; return { dataURLtoBlob: D, downloadURL: a }
    }); f(a, "Extensions/OfflineExporting.js", [a["Core/Chart/Chart.js"],
    a["Core/Globals.js"], a["Core/Renderer/SVG/SVGRenderer.js"], a["Core/Utilities.js"], a["Extensions/DownloadURL.js"]], function (a, b, f, e, L) {
        function u(a, d) { var g = k.getElementsByTagName("head")[0], c = k.createElement("script"); c.type = "text/javascript"; c.src = a; c.onload = d; c.onerror = function () { F("Error loading script " + a) }; g.appendChild(c) } function m(a) {
            var b = -1 < v.userAgent.indexOf("WebKit") && 0 > v.userAgent.indexOf("Chrome"); try {
                if (!b && 0 > v.userAgent.toLowerCase().indexOf("firefox")) return G.createObjectURL(new d.Blob([a],
                    { type: "image/svg+xml;charset-utf-16" }))
            } catch (g) { } return "data:image/svg+xml;charset=UTF-8," + encodeURIComponent(a)
        } function x(a, b, g, c, y, n, f, p, l) {
            var h = new d.Image, r = function () { setTimeout(function () { var d = k.createElement("canvas"), n = d.getContext && d.getContext("2d"); try { if (n) { d.height = h.height * c; d.width = h.width * c; n.drawImage(h, 0, 0, d.width, d.height); try { var w = d.toDataURL(b); y(w, b, g, c) } catch (I) { q(a, b, g, c) } } else f(a, b, g, c) } finally { l && l(a, b, g, c) } }, M) }, e = function () { p(a, b, g, c); l && l(a, b, g, c) }; var q = function () {
                h =
                    new d.Image; q = n; h.crossOrigin = "Anonymous"; h.onload = r; h.onerror = e; h.src = a
            }; h.onload = r; h.onerror = e; h.src = a
        } function C(a, b, g, c) {
            function f(a, b) { var c = a.width.baseVal.value + 2 * b; b = a.height.baseVal.value + 2 * b; c = new d.jsPDF(b > c ? "p" : "l", "pt", [c, b]);[].forEach.call(a.querySelectorAll('*[visibility="hidden"]'), function (a) { a.parentNode.removeChild(a) }); d.svg2pdf(a, c, { removeInvalid: !0 }); return c.output("datauristring") } function n() {
                l.innerHTML = a; var b = l.getElementsByTagName("text"), d;[].forEach.call(b, function (a) {
                    ["font-family",
                        "font-size"].forEach(function (b) { for (var c = a; c && c !== l;) { if (c.style[b]) { a.style[b] = c.style[b]; break } c = c.parentNode } }); a.style["font-family"] = a.style["font-family"] && a.style["font-family"].split(" ").splice(-1); d = a.getElementsByTagName("title");[].forEach.call(d, function (b) { a.removeChild(b) })
                }); b = f(l.firstChild, 0); try { z(b, r), c && c() } catch (N) { g(N) }
            } var e = !0, p = b.libURL || J().exporting.libURL, l = k.createElement("div"), h = b.type || "image/png", r = (b.filename || "chart") + "." + ("image/svg+xml" === h ? "svg" : h.split("/")[1]),
                A = b.scale || 1; p = "/" !== p.slice(-1) ? p + "/" : p; if ("image/svg+xml" === h) try { if ("undefined" !== typeof v.msSaveOrOpenBlob) { var q = new MSBlobBuilder; q.append(a); var t = q.getBlob("image/svg+xml") } else t = m(a); z(t, r); c && c() } catch (w) { g(w) } else if ("application/pdf" === h) d.jsPDF && d.svg2pdf ? n() : (e = !0, u(p + "jspdf.js", function () { u(p + "svg2pdf.js", function () { n() }) })); else {
                    t = m(a); var H = function () { try { G.revokeObjectURL(t) } catch (w) { } }; x(t, h, {}, A, function (a) { try { z(a, r), c && c() } catch (I) { g(I) } }, function () {
                        var b = k.createElement("canvas"),
                            n = b.getContext("2d"), f = a.match(/^<svg[^>]*width\s*=\s*"?(\d+)"?[^>]*>/)[1] * A, y = a.match(/^<svg[^>]*height\s*=\s*"?(\d+)"?[^>]*>/)[1] * A, l = function () { n.drawSvg(a, 0, 0, f, y); try { z(v.msSaveOrOpenBlob ? b.msToBlob() : b.toDataURL(h), r), c && c() } catch (O) { g(O) } finally { H() } }; b.width = f; b.height = y; d.canvg ? l() : (e = !0, u(p + "rgbcolor.js", function () { u(p + "canvg.js", function () { l() }) }))
                    }, g, g, function () { e && H() })
                }
        } var d = b.win, k = b.doc, E = e.addEvent, F = e.error, P = e.extend, J = e.getOptions, K = e.merge, z = L.downloadURL, G = d.URL || d.webkitURL ||
            d, v = d.navigator, B = /Edge\/|Trident\/|MSIE /.test(v.userAgent), M = B ? 150 : 0; b.CanVGRenderer = {}; a.prototype.getSVGForLocalExport = function (a, b, d, c) {
                var g = this, n = 0, f, e, l, h, k = function () { n === q.length && c(g.sanitizeSVG(f.innerHTML, e)) }, r = function (a, b, c) { ++n; c.imageElement.setAttributeNS("http://www.w3.org/1999/xlink", "href", a); k() }; g.unbindGetSVG = E(g, "getSVG", function (a) { e = a.chartCopy.options; f = a.chartCopy.container.cloneNode(!0) }); g.getSVGForExport(a, b); var q = f.getElementsByTagName("image"); try {
                    if (!q.length) {
                        c(g.sanitizeSVG(f.innerHTML,
                            e)); return
                    } var t = 0; for (l = q.length; t < l; ++t) { var m = q[t]; (h = m.getAttributeNS("http://www.w3.org/1999/xlink", "href")) ? x(h, "image/png", { imageElement: m }, a.scale, r, d, d, d) : (++n, m.parentNode.removeChild(m), k()) }
                } catch (w) { d(w) } g.unbindGetSVG()
            }; a.prototype.exportChartLocal = function (a, b) {
                var d = this, c = K(d.options.exporting, a), e = function (a) { !1 === c.fallbackToExportServer ? c.error ? c.error(c, a) : F(28, !0) : d.exportChart(c) }; a = function () {
                    return [].some.call(d.container.getElementsByTagName("image"), function (a) {
                        a = a.getAttribute("href");
                        return "" !== a && 0 !== a.indexOf("data:")
                    })
                }; B && d.styledMode && (f.prototype.inlineWhitelist = [/^blockSize/, /^border/, /^caretColor/, /^color/, /^columnRule/, /^columnRuleColor/, /^cssFloat/, /^cursor/, /^fill$/, /^fillOpacity/, /^font/, /^inlineSize/, /^length/, /^lineHeight/, /^opacity/, /^outline/, /^parentRule/, /^rx$/, /^ry$/, /^stroke/, /^textAlign/, /^textAnchor/, /^textDecoration/, /^transform/, /^vectorEffect/, /^visibility/, /^x$/, /^y$/]); B && ("application/pdf" === c.type || d.container.getElementsByTagName("image").length &&
                    "image/svg+xml" !== c.type) || "application/pdf" === c.type && a() ? e("Image type not supported for this chart/browser.") : d.getSVGForLocalExport(c, b, e, function (a) { -1 < a.indexOf("<foreignObject") && "image/svg+xml" !== c.type ? e("Image type not supportedfor charts with embedded HTML") : C(a, P({ filename: d.getFilename() }, c), e) })
            }; K(!0, J().exporting, {
                libURL: "https://code.highcharts.com/8.2.2/lib/", menuItemDefinitions: {
                    downloadPNG: { textKey: "downloadPNG", onclick: function () { this.exportChartLocal() } }, downloadJPEG: {
                        textKey: "downloadJPEG",
                        onclick: function () { this.exportChartLocal({ type: "image/jpeg" }) }
                    }, downloadSVG: { textKey: "downloadSVG", onclick: function () { this.exportChartLocal({ type: "image/svg+xml" }) } }, downloadPDF: { textKey: "downloadPDF", onclick: function () { this.exportChartLocal({ type: "application/pdf" }) } }
                }
            }); b.downloadSVGLocal = C
    }); f(a, "masters/modules/offline-exporting.src.js", [], function () { })
});
//# sourceMappingURL=offline-exporting.js.map

