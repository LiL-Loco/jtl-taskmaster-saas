'use client'

import { useJobs } from '@/hooks/useJobs'

export default function JobsList() {
  const { data: jobs, isLoading, error } = useJobs()

  if (isLoading) return <div>Loading...</div>
  if (error) return <div>Error: {error.message}</div>
  
  return (
    <div>
      <h2>Jobs List</h2>
      {jobs && jobs.length > 0 ? (
        <ul>
          {jobs.map((job) => (
            <li key={job.id}>{job.name || 'Unnamed Job'}</li>
          ))}
        </ul>
      ) : (
        <p>No jobs found</p>
      )}
    </div>
  )
}