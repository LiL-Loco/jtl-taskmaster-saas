// hooks/useSignalR.ts
import { useEffect } from 'react'
import { useQueryClient } from '@tanstack/react-query'
import { signalRService } from '@/services/signalr'
import { Job, JobStatus, TaskStatus } from '@/types/api'

export function useSignalR() {
  const queryClient = useQueryClient()

  useEffect(() => {
    signalRService.start()

    const jobStatusUnsubscribe = signalRService.onJobStatusUpdated(
      (jobId: string, status: JobStatus) => {
        // Update job status in cache
        queryClient.setQueryData(['jobs'], (oldData: Job[] | undefined) => {
          if (!oldData) return oldData
          return oldData.map(job => 
            job.id === jobId ? { ...job, status } : job
          )
        })

        // Update single job cache if exists
        queryClient.setQueryData(['jobs', jobId], (oldData: Job | undefined) => {
          if (!oldData) return oldData
          return { ...oldData, status }
        })
      }
    )

    const taskStatusUnsubscribe = signalRService.onTaskStatusUpdated(
      (jobId: string, taskId: string, status: TaskStatus) => {
        // Update job cache to reflect task status
        queryClient.setQueryData(['jobs', jobId], (oldData: Job | undefined) => {
          if (!oldData) return oldData
          return {
            ...oldData,
            tasks: oldData.tasks.map(task =>
              task.id === taskId ? { ...task, status } : task
            )
          }
        })
      }
    )

    return () => {
      jobStatusUnsubscribe()
      taskStatusUnsubscribe()
      signalRService.stop()
    }
  }, [queryClient])
}