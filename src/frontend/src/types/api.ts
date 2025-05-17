// src/types/api.ts

// Basis-Typen
export type JobStatus = 'Pending' | 'Running' | 'Completed' | 'Failed';
export type TaskStatus = 'Pending' | 'Running' | 'Completed' | 'Failed';

// Entitäten
export interface BaseEntity {
    id: string;
    created: string;
    modified: string;
}

export interface Job extends BaseEntity {
    name: string;
    description?: string;
    isEnabled: boolean;
    status: JobStatus;
    lastRun?: string;
    tasks: Task[];
    tenantId: string;
}

export interface Task extends BaseEntity {
    jobId: string;
    type: string;
    parameters: string;
    order: number;
    isEnabled: boolean;
    status?: TaskStatus;
    retryCount?: number;
    nextRetry?: string;
    lastError?: string;
}

// API Responses
export interface ApiResponse<T> {
    data: T;
    message?: string;
    errors?: string[];
}

// API Requests
export interface CreateJobRequest {
    name: string;
    description?: string;
    isEnabled: boolean;
    tasks: CreateTaskRequest[];
}

export interface CreateTaskRequest {
    type: string;
    parameters: string;
    order: number;
    isEnabled: boolean;
}

// Agent Types
export interface AgentInfo {
    id: string;
    machineId: string;
    isConnected: boolean;
    lastSeen: string;
    supportedTaskTypes: string[];
}

export interface AgentStats {
    cpuUsage: number;
    memoryUsage: number;
    runningTasks: number;
}

// Task Execution
export interface TaskExecutionRequest {
    taskId: string;
    type: string;
    parameters: string;
}

export interface TaskExecutionResult {
    success: boolean;
    errorMessage?: string;
}

// Monitoring
export interface SystemStats {
    totalJobs: number;
    activeJobs: number;
    failedJobs: number;
    connectedAgents: number;
    totalAgents: number;
}

export interface JobExecutionHistory {
    jobId: string;
    executions: JobExecution[];
}

export interface JobExecution {
    startTime: string;
    endTime?: string;
    status: JobStatus;
    error?: string;
    taskExecutions: TaskExecution[];
}

export interface TaskExecution {
    taskId: string;
    startTime: string;
    endTime?: string;
    status: TaskStatus;
    error?: string;
}
