'use client'

// src/hooks/useJobs.ts
import { useQuery } from '@tanstack/react-query'
import { jobsApi } from '@/services/api'

export function useJobs() {
  return useQuery({
    queryKey: ['jobs'],
    queryFn: () => jobsApi.getAll(),
  })
}
