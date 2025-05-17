(globalThis.TURBOPACK = globalThis.TURBOPACK || []).push([typeof document === "object" ? document.currentScript : undefined, {

"[project]/src/services/signalr.ts [app-client] (ecmascript)": ((__turbopack_context__) => {
"use strict";

var { g: global, __dirname, k: __turbopack_refresh__, m: module } = __turbopack_context__;
{
// services/signalr.ts
__turbopack_context__.s({
    "SignalRService": (()=>SignalRService),
    "signalRService": (()=>signalRService)
});
var __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f$next$2f$dist$2f$build$2f$polyfills$2f$process$2e$js__$5b$app$2d$client$5d$__$28$ecmascript$29$__ = __turbopack_context__.i("[project]/node_modules/next/dist/build/polyfills/process.js [app-client] (ecmascript)");
var __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f40$microsoft$2f$signalr$2f$dist$2f$esm$2f$HubConnectionBuilder$2e$js__$5b$app$2d$client$5d$__$28$ecmascript$29$__ = __turbopack_context__.i("[project]/node_modules/@microsoft/signalr/dist/esm/HubConnectionBuilder.js [app-client] (ecmascript)");
;
class SignalRService {
    hubConnection;
    statusCallbacks = [];
    taskStatusCallbacks = [];
    constructor(){
        this.hubConnection = new __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f40$microsoft$2f$signalr$2f$dist$2f$esm$2f$HubConnectionBuilder$2e$js__$5b$app$2d$client$5d$__$28$ecmascript$29$__["HubConnectionBuilder"]().withUrl(`${("TURBOPACK compile-time value", "http://localhost:5000/api")}/hubs/jobs`).withAutomaticReconnect().build();
        this.hubConnection.on('JobStatusUpdated', (jobId, status)=>{
            this.statusCallbacks.forEach((callback)=>callback(jobId, status));
        });
        this.hubConnection.on('TaskStatusUpdated', (jobId, taskId, status)=>{
            this.taskStatusCallbacks.forEach((callback)=>callback(jobId, taskId, status));
        });
    }
    async start() {
        try {
            await this.hubConnection.start();
            console.log('SignalR Connected');
        } catch (err) {
            console.error('SignalR Connection Error: ', err);
            setTimeout(()=>this.start(), 5000);
        }
    }
    onJobStatusUpdated(callback) {
        this.statusCallbacks.push(callback);
        return ()=>{
            this.statusCallbacks = this.statusCallbacks.filter((cb)=>cb !== callback);
        };
    }
    onTaskStatusUpdated(callback) {
        this.taskStatusCallbacks.push(callback);
        return ()=>{
            this.taskStatusCallbacks = this.taskStatusCallbacks.filter((cb)=>cb !== callback);
        };
    }
    async stop() {
        await this.hubConnection.stop();
    }
}
const signalRService = new SignalRService();
if (typeof globalThis.$RefreshHelpers$ === 'object' && globalThis.$RefreshHelpers !== null) {
    __turbopack_context__.k.registerExports(module, globalThis.$RefreshHelpers$);
}
}}),
"[project]/src/hooks/useSignalR.ts [app-client] (ecmascript)": ((__turbopack_context__) => {
"use strict";

var { g: global, __dirname, k: __turbopack_refresh__, m: module } = __turbopack_context__;
{
// hooks/useSignalR.ts
__turbopack_context__.s({
    "useSignalR": (()=>useSignalR)
});
var __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f$next$2f$dist$2f$compiled$2f$react$2f$index$2e$js__$5b$app$2d$client$5d$__$28$ecmascript$29$__ = __turbopack_context__.i("[project]/node_modules/next/dist/compiled/react/index.js [app-client] (ecmascript)");
var __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f40$tanstack$2f$react$2d$query$2f$build$2f$modern$2f$QueryClientProvider$2e$js__$5b$app$2d$client$5d$__$28$ecmascript$29$__ = __turbopack_context__.i("[project]/node_modules/@tanstack/react-query/build/modern/QueryClientProvider.js [app-client] (ecmascript)");
var __TURBOPACK__imported__module__$5b$project$5d2f$src$2f$services$2f$signalr$2e$ts__$5b$app$2d$client$5d$__$28$ecmascript$29$__ = __turbopack_context__.i("[project]/src/services/signalr.ts [app-client] (ecmascript)");
var _s = __turbopack_context__.k.signature();
;
;
;
function useSignalR() {
    _s();
    const queryClient = (0, __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f40$tanstack$2f$react$2d$query$2f$build$2f$modern$2f$QueryClientProvider$2e$js__$5b$app$2d$client$5d$__$28$ecmascript$29$__["useQueryClient"])();
    (0, __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f$next$2f$dist$2f$compiled$2f$react$2f$index$2e$js__$5b$app$2d$client$5d$__$28$ecmascript$29$__["useEffect"])({
        "useSignalR.useEffect": ()=>{
            __TURBOPACK__imported__module__$5b$project$5d2f$src$2f$services$2f$signalr$2e$ts__$5b$app$2d$client$5d$__$28$ecmascript$29$__["signalRService"].start();
            const jobStatusUnsubscribe = __TURBOPACK__imported__module__$5b$project$5d2f$src$2f$services$2f$signalr$2e$ts__$5b$app$2d$client$5d$__$28$ecmascript$29$__["signalRService"].onJobStatusUpdated({
                "useSignalR.useEffect.jobStatusUnsubscribe": (jobId, status)=>{
                    // Update job status in cache
                    queryClient.setQueryData([
                        'jobs'
                    ], {
                        "useSignalR.useEffect.jobStatusUnsubscribe": (oldData)=>{
                            if (!oldData) return oldData;
                            return oldData.map({
                                "useSignalR.useEffect.jobStatusUnsubscribe": (job)=>job.id === jobId ? {
                                        ...job,
                                        status
                                    } : job
                            }["useSignalR.useEffect.jobStatusUnsubscribe"]);
                        }
                    }["useSignalR.useEffect.jobStatusUnsubscribe"]);
                    // Update single job cache if exists
                    queryClient.setQueryData([
                        'jobs',
                        jobId
                    ], {
                        "useSignalR.useEffect.jobStatusUnsubscribe": (oldData)=>{
                            if (!oldData) return oldData;
                            return {
                                ...oldData,
                                status
                            };
                        }
                    }["useSignalR.useEffect.jobStatusUnsubscribe"]);
                }
            }["useSignalR.useEffect.jobStatusUnsubscribe"]);
            const taskStatusUnsubscribe = __TURBOPACK__imported__module__$5b$project$5d2f$src$2f$services$2f$signalr$2e$ts__$5b$app$2d$client$5d$__$28$ecmascript$29$__["signalRService"].onTaskStatusUpdated({
                "useSignalR.useEffect.taskStatusUnsubscribe": (jobId, taskId, status)=>{
                    // Update job cache to reflect task status
                    queryClient.setQueryData([
                        'jobs',
                        jobId
                    ], {
                        "useSignalR.useEffect.taskStatusUnsubscribe": (oldData)=>{
                            if (!oldData) return oldData;
                            return {
                                ...oldData,
                                tasks: oldData.tasks.map({
                                    "useSignalR.useEffect.taskStatusUnsubscribe": (task)=>task.id === taskId ? {
                                            ...task,
                                            status
                                        } : task
                                }["useSignalR.useEffect.taskStatusUnsubscribe"])
                            };
                        }
                    }["useSignalR.useEffect.taskStatusUnsubscribe"]);
                }
            }["useSignalR.useEffect.taskStatusUnsubscribe"]);
            return ({
                "useSignalR.useEffect": ()=>{
                    jobStatusUnsubscribe();
                    taskStatusUnsubscribe();
                    __TURBOPACK__imported__module__$5b$project$5d2f$src$2f$services$2f$signalr$2e$ts__$5b$app$2d$client$5d$__$28$ecmascript$29$__["signalRService"].stop();
                }
            })["useSignalR.useEffect"];
        }
    }["useSignalR.useEffect"], [
        queryClient
    ]);
}
_s(useSignalR, "aixO0mo1bdM1nWLRolCdKzppx/I=", false, function() {
    return [
        __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f40$tanstack$2f$react$2d$query$2f$build$2f$modern$2f$QueryClientProvider$2e$js__$5b$app$2d$client$5d$__$28$ecmascript$29$__["useQueryClient"]
    ];
});
if (typeof globalThis.$RefreshHelpers$ === 'object' && globalThis.$RefreshHelpers !== null) {
    __turbopack_context__.k.registerExports(module, globalThis.$RefreshHelpers$);
}
}}),
"[project]/src/components/providers/layout-provider.tsx [app-client] (ecmascript)": ((__turbopack_context__) => {
"use strict";

var { g: global, __dirname, k: __turbopack_refresh__, m: module } = __turbopack_context__;
{
// components/providers/layout-provider.tsx
__turbopack_context__.s({
    "LayoutProvider": (()=>LayoutProvider)
});
var __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f$next$2f$dist$2f$compiled$2f$react$2f$jsx$2d$dev$2d$runtime$2e$js__$5b$app$2d$client$5d$__$28$ecmascript$29$__ = __turbopack_context__.i("[project]/node_modules/next/dist/compiled/react/jsx-dev-runtime.js [app-client] (ecmascript)");
var __TURBOPACK__imported__module__$5b$project$5d2f$src$2f$hooks$2f$useSignalR$2e$ts__$5b$app$2d$client$5d$__$28$ecmascript$29$__ = __turbopack_context__.i("[project]/src/hooks/useSignalR.ts [app-client] (ecmascript)");
var __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f40$tanstack$2f$query$2d$core$2f$build$2f$modern$2f$queryClient$2e$js__$5b$app$2d$client$5d$__$28$ecmascript$29$__ = __turbopack_context__.i("[project]/node_modules/@tanstack/query-core/build/modern/queryClient.js [app-client] (ecmascript)");
var __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f40$tanstack$2f$react$2d$query$2f$build$2f$modern$2f$QueryClientProvider$2e$js__$5b$app$2d$client$5d$__$28$ecmascript$29$__ = __turbopack_context__.i("[project]/node_modules/@tanstack/react-query/build/modern/QueryClientProvider.js [app-client] (ecmascript)");
;
var _s = __turbopack_context__.k.signature();
'use client';
;
;
const queryClient = new __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f40$tanstack$2f$query$2d$core$2f$build$2f$modern$2f$queryClient$2e$js__$5b$app$2d$client$5d$__$28$ecmascript$29$__["QueryClient"]();
function LayoutProvider({ children }) {
    _s();
    (0, __TURBOPACK__imported__module__$5b$project$5d2f$src$2f$hooks$2f$useSignalR$2e$ts__$5b$app$2d$client$5d$__$28$ecmascript$29$__["useSignalR"])();
    return /*#__PURE__*/ (0, __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f$next$2f$dist$2f$compiled$2f$react$2f$jsx$2d$dev$2d$runtime$2e$js__$5b$app$2d$client$5d$__$28$ecmascript$29$__["jsxDEV"])(__TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f40$tanstack$2f$react$2d$query$2f$build$2f$modern$2f$QueryClientProvider$2e$js__$5b$app$2d$client$5d$__$28$ecmascript$29$__["QueryClientProvider"], {
        client: queryClient,
        children: children
    }, void 0, false, {
        fileName: "[project]/src/components/providers/layout-provider.tsx",
        lineNumber: 14,
        columnNumber: 5
    }, this);
}
_s(LayoutProvider, "jut3UnVgAEdQyNCoN8p9FING8nI=", false, function() {
    return [
        __TURBOPACK__imported__module__$5b$project$5d2f$src$2f$hooks$2f$useSignalR$2e$ts__$5b$app$2d$client$5d$__$28$ecmascript$29$__["useSignalR"]
    ];
});
_c = LayoutProvider;
var _c;
__turbopack_context__.k.register(_c, "LayoutProvider");
if (typeof globalThis.$RefreshHelpers$ === 'object' && globalThis.$RefreshHelpers !== null) {
    __turbopack_context__.k.registerExports(module, globalThis.$RefreshHelpers$);
}
}}),
"[project]/src/components/ui/sonner.tsx [app-client] (ecmascript)": ((__turbopack_context__) => {
"use strict";

var { g: global, __dirname, k: __turbopack_refresh__, m: module } = __turbopack_context__;
{
__turbopack_context__.s({
    "Toaster": (()=>Toaster)
});
var __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f$next$2f$dist$2f$compiled$2f$react$2f$jsx$2d$dev$2d$runtime$2e$js__$5b$app$2d$client$5d$__$28$ecmascript$29$__ = __turbopack_context__.i("[project]/node_modules/next/dist/compiled/react/jsx-dev-runtime.js [app-client] (ecmascript)");
var __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f$next$2d$themes$2f$dist$2f$index$2e$mjs__$5b$app$2d$client$5d$__$28$ecmascript$29$__ = __turbopack_context__.i("[project]/node_modules/next-themes/dist/index.mjs [app-client] (ecmascript)");
var __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f$sonner$2f$dist$2f$index$2e$mjs__$5b$app$2d$client$5d$__$28$ecmascript$29$__ = __turbopack_context__.i("[project]/node_modules/sonner/dist/index.mjs [app-client] (ecmascript)");
;
var _s = __turbopack_context__.k.signature();
"use client";
;
;
const Toaster = ({ ...props })=>{
    _s();
    const { theme = "system" } = (0, __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f$next$2d$themes$2f$dist$2f$index$2e$mjs__$5b$app$2d$client$5d$__$28$ecmascript$29$__["useTheme"])();
    return /*#__PURE__*/ (0, __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f$next$2f$dist$2f$compiled$2f$react$2f$jsx$2d$dev$2d$runtime$2e$js__$5b$app$2d$client$5d$__$28$ecmascript$29$__["jsxDEV"])(__TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f$sonner$2f$dist$2f$index$2e$mjs__$5b$app$2d$client$5d$__$28$ecmascript$29$__["Toaster"], {
        theme: theme,
        className: "toaster group",
        style: {
            "--normal-bg": "var(--popover)",
            "--normal-text": "var(--popover-foreground)",
            "--normal-border": "var(--border)"
        },
        ...props
    }, void 0, false, {
        fileName: "[project]/src/components/ui/sonner.tsx",
        lineNumber: 10,
        columnNumber: 5
    }, this);
};
_s(Toaster, "EriOrahfenYKDCErPq+L6926Dw4=", false, function() {
    return [
        __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f$next$2d$themes$2f$dist$2f$index$2e$mjs__$5b$app$2d$client$5d$__$28$ecmascript$29$__["useTheme"]
    ];
});
_c = Toaster;
;
var _c;
__turbopack_context__.k.register(_c, "Toaster");
if (typeof globalThis.$RefreshHelpers$ === 'object' && globalThis.$RefreshHelpers !== null) {
    __turbopack_context__.k.registerExports(module, globalThis.$RefreshHelpers$);
}
}}),
}]);

//# sourceMappingURL=src_5cd99068._.js.map