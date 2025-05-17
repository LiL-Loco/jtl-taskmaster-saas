// src/frontend/src/app/(dashboard)/jobs/new/page.tsx
'use client'

import { JobForm } from "@/components/jobs/job-form"

export default function NewJobPage() {
  return (
    <div className="space-y-6">
      <h1 className="text-3xl font-bold">Neuer Job</h1>
      <JobForm />
    </div>
  )
}
