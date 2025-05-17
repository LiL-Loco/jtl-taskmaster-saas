// services/signalr.ts
import * as signalR from '@microsoft/signalr'
import { JobStatus, TaskStatus } from '@/types/api'

export class SignalRService {
  private hubConnection: signalR.HubConnection
  private statusCallbacks: ((jobId: string, status: JobStatus) => void)[] = []
  private taskStatusCallbacks: ((jobId: string, taskId: string, status: TaskStatus) => void)[] = []

  constructor() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${process.env.NEXT_PUBLIC_API_URL}/hubs/jobs`)
      .withAutomaticReconnect()
      .build()

    this.hubConnection.on('JobStatusUpdated', (jobId: string, status: JobStatus) => {
      this.statusCallbacks.forEach(callback => callback(jobId, status))
    })

    this.hubConnection.on('TaskStatusUpdated', 
      (jobId: string, taskId: string, status: TaskStatus) => {
        this.taskStatusCallbacks.forEach(callback => 
          callback(jobId, taskId, status))
    })
  }

  async start() {
    try {
      await this.hubConnection.start()
      console.log('SignalR Connected')
    } catch (err) {
      console.error('SignalR Connection Error: ', err)
      setTimeout(() => this.start(), 5000)
    }
  }

  onJobStatusUpdated(callback: (jobId: string, status: JobStatus) => void) {
    this.statusCallbacks.push(callback)
    return () => {
      this.statusCallbacks = this.statusCallbacks.filter(cb => cb !== callback)
    }
  }

  onTaskStatusUpdated(
    callback: (jobId: string, taskId: string, status: TaskStatus) => void
  ) {
    this.taskStatusCallbacks.push(callback)
    return () => {
      this.taskStatusCallbacks = this.taskStatusCallbacks.filter(cb => cb !== callback)
    }
  }

  async stop() {
    await this.hubConnection.stop()
  }
}

export const signalRService = new SignalRService()