module.exports = {

"[externals]/next/dist/compiled/next-server/app-page-turbo.runtime.dev.js [external] (next/dist/compiled/next-server/app-page-turbo.runtime.dev.js, cjs)": (function(__turbopack_context__) {

var { g: global, __dirname, m: module, e: exports } = __turbopack_context__;
{
const mod = __turbopack_context__.x("next/dist/compiled/next-server/app-page-turbo.runtime.dev.js", () => require("next/dist/compiled/next-server/app-page-turbo.runtime.dev.js"));

module.exports = mod;
}}),
"[project]/src/services/signalr.ts [app-ssr] (ecmascript)": ((__turbopack_context__) => {
"use strict";

var { g: global, __dirname } = __turbopack_context__;
{
// services/signalr.ts
__turbopack_context__.s({
    "SignalRService": (()=>SignalRService),
    "signalRService": (()=>signalRService)
});
var __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f40$microsoft$2f$signalr$2f$dist$2f$esm$2f$HubConnectionBuilder$2e$js__$5b$app$2d$ssr$5d$__$28$ecmascript$29$__ = __turbopack_context__.i("[project]/node_modules/@microsoft/signalr/dist/esm/HubConnectionBuilder.js [app-ssr] (ecmascript)");
;
class SignalRService {
    hubConnection;
    statusCallbacks = [];
    taskStatusCallbacks = [];
    constructor(){
        this.hubConnection = new __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f40$microsoft$2f$signalr$2f$dist$2f$esm$2f$HubConnectionBuilder$2e$js__$5b$app$2d$ssr$5d$__$28$ecmascript$29$__["HubConnectionBuilder"]().withUrl(`${("TURBOPACK compile-time value", "http://localhost:5000/api")}/hubs/jobs`).withAutomaticReconnect().build();
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
}}),
"[project]/src/hooks/useSignalR.ts [app-ssr] (ecmascript)": ((__turbopack_context__) => {
"use strict";

var { g: global, __dirname } = __turbopack_context__;
{
// hooks/useSignalR.ts
__turbopack_context__.s({
    "useSignalR": (()=>useSignalR)
});
var __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f$next$2f$dist$2f$server$2f$route$2d$modules$2f$app$2d$page$2f$vendored$2f$ssr$2f$react$2e$js__$5b$app$2d$ssr$5d$__$28$ecmascript$29$__ = __turbopack_context__.i("[project]/node_modules/next/dist/server/route-modules/app-page/vendored/ssr/react.js [app-ssr] (ecmascript)");
var __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f40$tanstack$2f$react$2d$query$2f$build$2f$modern$2f$QueryClientProvider$2e$js__$5b$app$2d$ssr$5d$__$28$ecmascript$29$__ = __turbopack_context__.i("[project]/node_modules/@tanstack/react-query/build/modern/QueryClientProvider.js [app-ssr] (ecmascript)");
var __TURBOPACK__imported__module__$5b$project$5d2f$src$2f$services$2f$signalr$2e$ts__$5b$app$2d$ssr$5d$__$28$ecmascript$29$__ = __turbopack_context__.i("[project]/src/services/signalr.ts [app-ssr] (ecmascript)");
;
;
;
function useSignalR() {
    const queryClient = (0, __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f40$tanstack$2f$react$2d$query$2f$build$2f$modern$2f$QueryClientProvider$2e$js__$5b$app$2d$ssr$5d$__$28$ecmascript$29$__["useQueryClient"])();
    (0, __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f$next$2f$dist$2f$server$2f$route$2d$modules$2f$app$2d$page$2f$vendored$2f$ssr$2f$react$2e$js__$5b$app$2d$ssr$5d$__$28$ecmascript$29$__["useEffect"])(()=>{
        __TURBOPACK__imported__module__$5b$project$5d2f$src$2f$services$2f$signalr$2e$ts__$5b$app$2d$ssr$5d$__$28$ecmascript$29$__["signalRService"].start();
        const jobStatusUnsubscribe = __TURBOPACK__imported__module__$5b$project$5d2f$src$2f$services$2f$signalr$2e$ts__$5b$app$2d$ssr$5d$__$28$ecmascript$29$__["signalRService"].onJobStatusUpdated((jobId, status)=>{
            // Update job status in cache
            queryClient.setQueryData([
                'jobs'
            ], (oldData)=>{
                if (!oldData) return oldData;
                return oldData.map((job)=>job.id === jobId ? {
                        ...job,
                        status
                    } : job);
            });
            // Update single job cache if exists
            queryClient.setQueryData([
                'jobs',
                jobId
            ], (oldData)=>{
                if (!oldData) return oldData;
                return {
                    ...oldData,
                    status
                };
            });
        });
        const taskStatusUnsubscribe = __TURBOPACK__imported__module__$5b$project$5d2f$src$2f$services$2f$signalr$2e$ts__$5b$app$2d$ssr$5d$__$28$ecmascript$29$__["signalRService"].onTaskStatusUpdated((jobId, taskId, status)=>{
            // Update job cache to reflect task status
            queryClient.setQueryData([
                'jobs',
                jobId
            ], (oldData)=>{
                if (!oldData) return oldData;
                return {
                    ...oldData,
                    tasks: oldData.tasks.map((task)=>task.id === taskId ? {
                            ...task,
                            status
                        } : task)
                };
            });
        });
        return ()=>{
            jobStatusUnsubscribe();
            taskStatusUnsubscribe();
            __TURBOPACK__imported__module__$5b$project$5d2f$src$2f$services$2f$signalr$2e$ts__$5b$app$2d$ssr$5d$__$28$ecmascript$29$__["signalRService"].stop();
        };
    }, [
        queryClient
    ]);
}
}}),
"[project]/src/components/providers/layout-provider.tsx [app-ssr] (ecmascript)": ((__turbopack_context__) => {
"use strict";

var { g: global, __dirname } = __turbopack_context__;
{
// components/providers/layout-provider.tsx
__turbopack_context__.s({
    "LayoutProvider": (()=>LayoutProvider)
});
var __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f$next$2f$dist$2f$server$2f$route$2d$modules$2f$app$2d$page$2f$vendored$2f$ssr$2f$react$2d$jsx$2d$dev$2d$runtime$2e$js__$5b$app$2d$ssr$5d$__$28$ecmascript$29$__ = __turbopack_context__.i("[project]/node_modules/next/dist/server/route-modules/app-page/vendored/ssr/react-jsx-dev-runtime.js [app-ssr] (ecmascript)");
var __TURBOPACK__imported__module__$5b$project$5d2f$src$2f$hooks$2f$useSignalR$2e$ts__$5b$app$2d$ssr$5d$__$28$ecmascript$29$__ = __turbopack_context__.i("[project]/src/hooks/useSignalR.ts [app-ssr] (ecmascript)");
var __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f40$tanstack$2f$query$2d$core$2f$build$2f$modern$2f$queryClient$2e$js__$5b$app$2d$ssr$5d$__$28$ecmascript$29$__ = __turbopack_context__.i("[project]/node_modules/@tanstack/query-core/build/modern/queryClient.js [app-ssr] (ecmascript)");
var __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f40$tanstack$2f$react$2d$query$2f$build$2f$modern$2f$QueryClientProvider$2e$js__$5b$app$2d$ssr$5d$__$28$ecmascript$29$__ = __turbopack_context__.i("[project]/node_modules/@tanstack/react-query/build/modern/QueryClientProvider.js [app-ssr] (ecmascript)");
'use client';
;
;
;
const queryClient = new __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f40$tanstack$2f$query$2d$core$2f$build$2f$modern$2f$queryClient$2e$js__$5b$app$2d$ssr$5d$__$28$ecmascript$29$__["QueryClient"]();
function LayoutProvider({ children }) {
    (0, __TURBOPACK__imported__module__$5b$project$5d2f$src$2f$hooks$2f$useSignalR$2e$ts__$5b$app$2d$ssr$5d$__$28$ecmascript$29$__["useSignalR"])();
    return /*#__PURE__*/ (0, __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f$next$2f$dist$2f$server$2f$route$2d$modules$2f$app$2d$page$2f$vendored$2f$ssr$2f$react$2d$jsx$2d$dev$2d$runtime$2e$js__$5b$app$2d$ssr$5d$__$28$ecmascript$29$__["jsxDEV"])(__TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f40$tanstack$2f$react$2d$query$2f$build$2f$modern$2f$QueryClientProvider$2e$js__$5b$app$2d$ssr$5d$__$28$ecmascript$29$__["QueryClientProvider"], {
        client: queryClient,
        children: children
    }, void 0, false, {
        fileName: "[project]/src/components/providers/layout-provider.tsx",
        lineNumber: 14,
        columnNumber: 5
    }, this);
}
}}),

};

//# sourceMappingURL=%5Broot-of-the-server%5D__b7fbeee2._.js.map