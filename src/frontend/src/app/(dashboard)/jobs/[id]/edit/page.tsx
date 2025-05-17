'use client'

import { useParams } from 'next/navigation'
import { useQuery } from '@tanstack/react-query'
import { JobForm } from '@/components/jobs/job-form'
import { useParams } from "next/navigation"
import { JobDetails } from "@/components/jobs/job-details"
import { jobsApi } from '@/services/api'
import { LoadingSpinner } from '@/components/ui/loading-spinner'

export default function EditJobPage() {
  const params = useParams()
  const jobId = params.id as string

  const { data: job, isLoading } = useQuery({
    queryKey: ['jobs', jobId],
    queryFn: () => jobsApi.getById(jobId)
  })

  if (isLoading) {
    return <LoadingSpinner />
  }

  return (
    <div className="flex flex-col gap-8">
      <h1 className="text-3xl font-bold">Edit Job</h1>
      <JobForm initialData={job} />
    </div>
  )
}