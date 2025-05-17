// src/frontend/src/app/(dashboard)/agents/page.tsx
'use client'

import { AgentList } from "@/components/agents/agent-list"

export default function AgentsPage() {
  return (
    <div className="space-y-6">
      <h1 className="text-3xl font-bold">Agenten</h1>
      <AgentList />
    </div>
  )
}
