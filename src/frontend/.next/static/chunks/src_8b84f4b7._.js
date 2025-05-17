(globalThis.TURBOPACK = globalThis.TURBOPACK || []).push([typeof document === "object" ? document.currentScript : undefined, {

"[project]/src/services/api.ts [app-client] (ecmascript)": ((__turbopack_context__) => {
"use strict";

var { g: global, __dirname, k: __turbopack_refresh__, m: module } = __turbopack_context__;
{
// services/api.ts
__turbopack_context__.s({
    "agentsApi": (()=>agentsApi),
    "authApi": (()=>authApi),
    "jobsApi": (()=>jobsApi),
    "tasksApi": (()=>tasksApi)
});
var __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f$next$2f$dist$2f$build$2f$polyfills$2f$process$2e$js__$5b$app$2d$client$5d$__$28$ecmascript$29$__ = __turbopack_context__.i("[project]/node_modules/next/dist/build/polyfills/process.js [app-client] (ecmascript)");
var __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f$axios$2f$lib$2f$axios$2e$js__$5b$app$2d$client$5d$__$28$ecmascript$29$__ = __turbopack_context__.i("[project]/node_modules/axios/lib/axios.js [app-client] (ecmascript)");
;
const api = __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f$axios$2f$lib$2f$axios$2e$js__$5b$app$2d$client$5d$__$28$ecmascript$29$__["default"].create({
    baseURL: ("TURBOPACK compile-time value", "http://localhost:5000/api"),
    headers: {
        'Content-Type': 'application/json'
    }
});
api.interceptors.request.use((config)=>{
    const token = localStorage.getItem('token');
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
});
api.interceptors.response.use((response)=>response, (error)=>{
    if (error.response?.status === 401) {
        // Handle token expiration
        localStorage.removeItem('token');
        window.location.href = '/login';
    }
    return Promise.reject(error);
});
const jobsApi = {
    getAll: ()=>api.get('/jobs').then((res)=>res.data.data),
    getById: (id)=>api.get('/jobs/').then((res)=>res.data.data),
    create: (job)=>api.post('/jobs', job).then((res)=>res.data.data),
    update: (id, job)=>api.put('/jobs/', job).then((res)=>res.data.data),
    delete: (id)=>api.delete('/jobs/'),
    start: (id)=>api.post('/jobs//start')
};
const tasksApi = {
    updateStatus: (jobId, taskId, status)=>api.put('/jobs//tasks//status', {
            status
        })
};
const agentsApi = {
    getAll: ()=>api.get('/agents').then((res)=>res.data.data),
    getStatus: (id)=>api.get('/agents//status').then((res)=>res.data.data)
};
const authApi = {
    login: (credentials)=>api.post('/auth/login', credentials).then((res)=>res.data.data),
    register: (userData)=>api.post('/auth/register', userData).then((res)=>res.data.data),
    refreshToken: ()=>api.post('/auth/refresh-token').then((res)=>res.data.data)
};
if (typeof globalThis.$RefreshHelpers$ === 'object' && globalThis.$RefreshHelpers !== null) {
    __turbopack_context__.k.registerExports(module, globalThis.$RefreshHelpers$);
}
}}),
"[project]/src/hooks/useJobs.ts [app-client] (ecmascript)": ((__turbopack_context__) => {
"use strict";

var { g: global, __dirname, k: __turbopack_refresh__, m: module } = __turbopack_context__;
{
__turbopack_context__.s({
    "useJobs": (()=>useJobs)
});
// src/hooks/useJobs.ts
var __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f40$tanstack$2f$react$2d$query$2f$build$2f$modern$2f$useQuery$2e$js__$5b$app$2d$client$5d$__$28$ecmascript$29$__ = __turbopack_context__.i("[project]/node_modules/@tanstack/react-query/build/modern/useQuery.js [app-client] (ecmascript)");
var __TURBOPACK__imported__module__$5b$project$5d2f$src$2f$services$2f$api$2e$ts__$5b$app$2d$client$5d$__$28$ecmascript$29$__ = __turbopack_context__.i("[project]/src/services/api.ts [app-client] (ecmascript)");
var _s = __turbopack_context__.k.signature();
'use client';
;
;
function useJobs() {
    _s();
    return (0, __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f40$tanstack$2f$react$2d$query$2f$build$2f$modern$2f$useQuery$2e$js__$5b$app$2d$client$5d$__$28$ecmascript$29$__["useQuery"])({
        queryKey: [
            'jobs'
        ],
        queryFn: {
            "useJobs.useQuery": ()=>__TURBOPACK__imported__module__$5b$project$5d2f$src$2f$services$2f$api$2e$ts__$5b$app$2d$client$5d$__$28$ecmascript$29$__["jobsApi"].getAll()
        }["useJobs.useQuery"]
    });
}
_s(useJobs, "4ZpngI1uv+Uo3WQHEZmTQ5FNM+k=", false, function() {
    return [
        __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f40$tanstack$2f$react$2d$query$2f$build$2f$modern$2f$useQuery$2e$js__$5b$app$2d$client$5d$__$28$ecmascript$29$__["useQuery"]
    ];
});
if (typeof globalThis.$RefreshHelpers$ === 'object' && globalThis.$RefreshHelpers !== null) {
    __turbopack_context__.k.registerExports(module, globalThis.$RefreshHelpers$);
}
}}),
}]);

//# sourceMappingURL=src_8b84f4b7._.js.map