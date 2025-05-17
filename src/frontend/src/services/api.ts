// services/api.ts
import axios, { AxiosError } from 'axios'
import { ApiResponse, Job, Task, AgentInfo, AgentStats } from '@/types/api'
import { Agent } from 'http'

const api = axios.create({
  baseURL: process.env.NEXT_PUBLIC_API_URL,
  headers: {
    'Content-Type': 'application/json',
  },
})

api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token')
  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }
  return config
})

api.interceptors.response.use(
  (response) => response,
  (error: AxiosError) => {
    if (error.response?.status === 401) {
      // Handle token expiration
      localStorage.removeItem('token')
      window.location.href = '/login'
    }
    return Promise.reject(error)
  }
)

export const jobsApi = {
  getAll: () => 
    api.get<ApiResponse<Job[]>>('/jobs').then(res => res.data.data),
  
  getById: (id: string) => 
    api.get<ApiResponse<Job>>('/jobs/').then(res => res.data.data),
  
  create: (job: Partial<Job>) => 
    api.post<ApiResponse<Job>>('/jobs', job).then(res => res.data.data),
  
  update: (id: string, job: Partial<Job>) => 
    api.put<ApiResponse<Job>>('/jobs/', job).then(res => res.data.data),
  
  delete: (id: string) => 
    api.delete('/jobs/'),
  
  start: (id: string) => 
    api.post('/jobs//start'),
}

export const tasksApi = {
  updateStatus: (jobId: string, taskId: string, status: string) =>
    api.put('/jobs//tasks//status', { status }),
}

export const agentsApi = {
  getAll: () => 
    api.get<ApiResponse<Agent[]>>('/agents').then(res => res.data.data),
  
  getStatus: (id: string) => 
    api.get<ApiResponse<AgentStats>>('/agents//status').then(res => res.data.data),
}

export const authApi = {
  login: (credentials: { email: string; password: string }) =>
    api.post<ApiResponse<{ token: string }>>('/auth/login', credentials)
      .then(res => res.data.data),
  
  register: (userData: { email: string; password: string; name: string }) =>
    api.post<ApiResponse<{ token: string }>>('/auth/register', userData)
      .then(res => res.data.data),
  
  refreshToken: () =>
    api.post<ApiResponse<{ token: string }>>('/auth/refresh-token')
      .then(res => res.data.data),
}